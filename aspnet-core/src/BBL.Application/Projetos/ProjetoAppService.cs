using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Expressions;
using Abp.Linq.Extensions;
using Abp.UI;
using BBL.Authorization;
using BBL.Enums;
using BBL.Helpers;
using BBL.Models.Fases;
using BBL.Models.Projetos;
using BBL.Models.Projetos.Service;
using BBL.Projetos.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBL.Projetos;

[AbpAuthorize]
public class ProjetoAppService : AsyncCrudAppService<Projeto, ProjetoDto, int, DefaultPagedDto, CreateProjetoDto, ProjetoDto>
{
    private readonly IProjetoService _projetoService;
    private readonly IRepository<Fase, int> _faseRepository;
    private readonly IRepository<Observacao, int> _observacaoRepository;
    public ProjetoAppService(
        IRepository<Projeto, int> repository,
        IRepository<Fase, int> faseRepository,
        IRepository<Observacao, int> observacaoRepository,
        IProjetoService projetoService) : base(repository)
    {
        _projetoService = projetoService;
        _faseRepository = faseRepository;
        _observacaoRepository = observacaoRepository;
    }

    protected override IQueryable<Projeto> CreateFilteredQuery(DefaultPagedDto input)
    {
        return base.CreateFilteredQuery(input)
                .Include(x => x.Cliente).ThenInclude(x => x.Endereco)
                .Include(x => x.Concessionaria)
                .Include(x => x.Fase)
                .Include(x => x.Tenant)
                .Include(x => x.Anexos)
                .WhereIf(!string.IsNullOrEmpty(input.SearchText),
                    x => x.Cliente.Nome.Contains(input.SearchText) ||
                        x.Tenant.Fantasia.Contains(input.SearchText) ||
                        x.Concessionaria.Nome.Contains(input.SearchText) ||
                        x.Fase.Nome.Contains(input.SearchText))
            ;
    }

    public override Task<ProjetoDto> CreateAsync(CreateProjetoDto input)
    {
        var fase = _faseRepository.GetAll().FirstOrDefault(x => x.Nome.Equals("Documento em Análise Técnica"));
        if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host)
        {
            input.FaseId = fase.Id;
            return base.CreateAsync(input);
        }
        else return base.CreateAsync(new CreateProjetoDto()
        {
            TenantId = AbpSession.TenantId,
            FaseId = fase.Id
        });
    }

    protected async override Task<Projeto> GetEntityByIdAsync(int id)
    {
        try
        {
            return Repository.GetAll()
                .Include(x => x.Cliente).ThenInclude(x => x.Endereco)
                .Include(x => x.Concessionaria)
                .Include(x => x.Fase)
                .Include(x => x.Anexos)
                .Include(x => x.Tenant).ThenInclude(x => x.ParceiroTabelasPreco).ThenInclude(x => x.TabelaPreco)
                .FirstOrDefault(x => x.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PagedResultDto<SearchProjetoDto>> GetAllListingAsync(DefaultPagedDto input)
    {
        CheckGetAllPermission();
        var query = CreateFilteredQuery(input);
        int totalCount = await AsyncQueryableExecuter.CountAsync(query).ConfigureAwait(continueOnCapturedContext: false);
        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);

        var listEntities = await AsyncQueryableExecuter.ToListAsync(query).ConfigureAwait(continueOnCapturedContext: false);
        return new PagedResultDto<SearchProjetoDto>(
            totalCount, ObjectMapper.Map<IReadOnlyList<SearchProjetoDto>>(listEntities));
    }

    public override async Task<ProjetoDto> UpdateAsync(ProjetoDto input)
    {
        try
        {
            bool atualizacaoFase = false;
            bool notificarCriacaoProjeto = false;

            Projeto projeto = Repository.Get(input.Id);

            if (projeto.FaseId == null)
            {
                if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Tenant) notificarCriacaoProjeto = true;
            }
            else if (projeto.FaseId != null && projeto.FaseId != input.FaseId)
            {
                var faseNew = _faseRepository.Get(input.FaseId);
                atualizacaoFase = NotificarAtualizacao(faseNew);
                Observacao observacao = new Observacao()
                {
                    Descricao = $"{DateTime.Now.Date:dd/MM/yyyy} - Atualização de Fase para: {faseNew.Nome}",
                    ProjetoId = projeto.Id
                };
                _observacaoRepository.Insert(observacao);
                if (faseNew.Nome.Equals("Projeto Homologado")) input.DataHomologacao = DateTime.Now;
            }

            var projetoUpdated = await base.UpdateAsync(input);
            _projetoService.CalcularValorProjeto(input.Id);
            if (atualizacaoFase) _projetoService.NotificarAtualizacaoFase(input.Id);
            if (notificarCriacaoProjeto) _projetoService.NotificarCriacaoProjeto(input.Id);
            CurrentUnitOfWork.SaveChanges();
            return projetoUpdated;
        }
        catch (Exception e)
        {
            throw new UserFriendlyException(e.Message);
        }
    }

    private bool NotificarAtualizacao(Fase faseNew)
    {
        // TODO -> CONFIRMAR COM ISMAEL QUAL NAO NOTIFICAR
        return faseNew.Nome.Equals("Documento Reprovado");
    }

    public IList<ProjetoContempladoDto> GetAllProjetosParaCobranca(int parceiroId, bool isVisualizacao)
    {
        var predicate = PredicateBuilder.New<Projeto>(true);
        if (!isVisualizacao) predicate.And(x => x.StatusPagamento == StatusPagamento.NaoFaturado && x.CobrancaId == null);

        IEnumerable<Projeto> projetos = Repository.GetAll()
            .AsNoTracking()
            .Include(x => x.Fase)
            .Include(x => x.Cliente)
            .Include(x => x.Concessionaria)
            .WhereIf(!isVisualizacao, predicate)
            .Where(x =>
                x.TenantId == parceiroId &&
                x.Fase.IncluiCobranca
            );

        List<ProjetoContempladoDto> projetosAContemplar = ObjectMapper.Map<List<ProjetoContempladoDto>>(projetos);
        CurrentUnitOfWork.SaveChanges();
        return projetosAContemplar;
    }

    [AbpAuthorize(PermissionNames.Pages_Admin)]
    public override Task DeleteAsync(EntityDto<int> input)
    {
        return base.DeleteAsync(input);
    }

    [HttpPost]
    public IList<SearchProjetoDto> GetAllProjetoByFilter(ProjetoFilterDto input)
    {

        List<Fase> fases = _faseRepository.GetAll().Where(x => input.Fases.Contains(x.Id)).ToList();

        List<Projeto> projetos = Repository.GetAll()
            .AsNoTracking()
            .Include(x => x.Fase)
            .Include(x => x.Cliente)
            .Include(x => x.Concessionaria)
            .Include(x => x.Tenant)
            .Include(x => x.Observacoes)
            .WhereIf(!input.Parceiros.IsNullOrEmpty(), x => input.Parceiros != null && input.Parceiros.Contains(x.Id))
            .Where(x => x.Observacoes.Any(x => x.CreationTime.Date >= input.DataInicio && x.CreationTime.Date <= input.DataFim))
            .ToList();
        ;

        List<Observacao> observacaos = _observacaoRepository.GetAll().Where(x => projetos.Select(x => x.Id).Contains(x.Id)).ToList();
        List<Observacao> observacaosDasFases = new List<Observacao>();

        foreach (Fase fase in fases)
        {
            observacaosDasFases.AddRange(observacaos.Where(x => x.Descricao.Contains(fase.Nome)));

        }

        IEnumerable<Projeto> projetosFiltrados = projetos.Where(x => observacaosDasFases.Select(x => x.Id).Contains(x.Id));
        List<SearchProjetoDto> searchProjetoDtos = ObjectMapper.Map<List<SearchProjetoDto>>(projetosFiltrados);
        return searchProjetoDtos;

    }

}