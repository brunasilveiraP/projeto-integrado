using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using BBL.Authorization;
using BBL.Cobrancas.Dto;
using BBL.Helpers;
using BBL.Models.Cobrancas;
using BBL.Models.Projetos;
using BBL.Projetos.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollectionExtensions = Abp.Collections.Extensions.CollectionExtensions;

namespace BBL.Cobrancas;

[AbpAuthorize(PermissionNames.Pages_Admin)]
public class CobrancaAppService : AsyncCrudAppService<Cobranca, CobrancaDto, int, DefaultPagedDto>
{
    private readonly IRepository<Projeto, int> _projetoRepository;

    public CobrancaAppService(
        IRepository<Cobranca, int> repository, IRepository<Projeto, int> projetoRepository) : base(repository)
    {
        _projetoRepository = projetoRepository;
    }

    protected override IQueryable<Cobranca> CreateFilteredQuery(DefaultPagedDto input)
    {
        return base.CreateFilteredQuery(input)
                .Include(x => x.Tenant)
                .Include(x => x.Projetos)
                .WhereIf(!string.IsNullOrEmpty(input.SearchText),
                    x => x.Tenant.Fantasia.Contains(input.SearchText))
            ;
    }

    protected async override Task<Cobranca> GetEntityByIdAsync(int id)
    {
        return Repository.GetAll()
                .Include(x => x.Tenant)
                .Include(x => x.Projetos)
                .FirstOrDefault(x => x.Id == id);
    }

    public override async Task<CobrancaDto> UpdateAsync(CobrancaDto input)
    {
        try
        {
            CheckUpdatePermission();
            var entity = await GetEntityByIdAsync(input.Id);
            var projects = entity.Projetos.Select(x => x.Id);
            var news = input.ProjetosId.Except(projects);
            var deleteds = projects.Except(input.ProjetosId);
            var novosProjetosUpdated = _projetoRepository.GetAll().Where(x => news.Contains(x.Id)).ToList();
            var desvincularCobranca = _projetoRepository.GetAll().Where(x => deleteds.Contains(x.Id)).ToList();

            foreach (var projetoNew in novosProjetosUpdated)
            {
                projetoNew.CobrancaId = input.Id;
                projetoNew.StatusPagamento = Enums.StatusPagamento.EmAberto;
                entity.StatusPagamento = Enums.StatusPagamento.EmAberto;
                _projetoRepository.Update(projetoNew);
            }

            foreach (var desvincular in desvincularCobranca)
            {
                desvincular.CobrancaId = null;
                _projetoRepository.Update(desvincular);
            }

            entity.ValorTotal = input.ValorTotal;
            entity.DataGeracao = input.DataGeracao;
            var cobranca = await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return ObjectMapper.Map<CobrancaDto>(cobranca);
        }
        catch (Exception e)
        {
            throw new UserFriendlyException(e.Message);
        }
    }

    public CobrancaDto ConfirmarPagamentoAsync(int cobrancaId)
    {
        try
        {
            Cobranca cobranca = Repository.GetAll()
                .Include(x => x.Projetos)
                .Include(x => x.Tenant)
                .FirstOrDefault(x => x.Id == cobrancaId);
            cobranca.DataPagamento = DateTime.Now;
            cobranca.StatusPagamento = Enums.StatusPagamento.Pago;
            var t = Repository.Update(cobranca);

            List<Projeto> projetos = _projetoRepository.GetAll().Where(x => x.CobrancaId == cobrancaId).ToList();
            foreach (var projeto in projetos)
            {
                projeto.StatusPagamento = Enums.StatusPagamento.Pago;
                _projetoRepository.Update(projeto);
            }

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<CobrancaDto>(cobranca);
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException(ex.Message);
        }
    }

    [HttpPost]
    public IEnumerable<CobrancaDto> GetAllCobrancaByFilter(CobrancaFilterDto input)
    {
        var cobrancas = Repository.GetAll()
                .Include(x => x.Tenant)
                .WhereIf(!CollectionExtensions.IsNullOrEmpty(input.Parceiros),
                    x => input.Parceiros.Contains(x.Tenant.Id))
                .WhereIf(input.Status != null, x => x.StatusPagamento == input.Status)
                .WhereIf(input.DataPagamentoInicio.HasValue && input.DataPagamentoFim.HasValue,
                    x => (input.DataPagamentoInicio.Value <= x.DataPagamento && input.DataPagamentoFim.Value >= x.DataPagamento))

                .WhereIf(input.DataInicio != null && input.DataFim != null, x =>
                    (input.DataInicio <= x.DataPagamento && input.DataFim >= x.DataPagamento)
                )
            ;
        return ObjectMapper.Map<IEnumerable<CobrancaDto>>(cobrancas);
    }

    public override Task DeleteAsync(EntityDto<int> input)
    {

        var projetos = _projetoRepository.GetAll().Where(x => x.CobrancaId == input.Id);

        foreach (var projeto in projetos)
        {
            projeto.CobrancaId = null;
            projeto.StatusPagamento = Enums.StatusPagamento.NaoFaturado;
            _projetoRepository.Update(projeto);
        }
        CurrentUnitOfWork.SaveChanges();

        return base.DeleteAsync(input);
    }
}