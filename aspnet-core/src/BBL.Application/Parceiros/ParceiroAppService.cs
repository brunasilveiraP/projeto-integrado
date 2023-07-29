using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using BBL.Authorization;
using BBL.Helpers;
using BBL.Models.Parceiros;
using BBL.Parceiros.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBL.Parceiros;

[AbpAuthorize]
public class ParceiroAppService : AsyncCrudAppService<Parceiro, ParceiroDto, int, DefaultPagedDto, ParceiroCreateDto, ParceiroCreateDto>
{
    public ParceiroAppService(IRepository<Parceiro, int> repository) : base(repository)
    {
    }

    protected override IQueryable<Parceiro> CreateFilteredQuery(DefaultPagedDto input)
    {
        int? tenantId = AbpSession.TenantId;
        return base.CreateFilteredQuery(input)
           .Include(x => x.ParceiroTabelasPreco).ThenInclude(x => x.TabelaPreco)
           .Include(x => x.DiasPagamento)
           .Include(x => x.Endereco)
           .WhereIf(AbpSession.TenantId != null, x => x.Id == AbpSession.TenantId)
           .WhereIf(!string.IsNullOrEmpty(input.SearchText),
           x => x.Fantasia.Contains(input.SearchText) ||
               x.RazaoSocial.Contains(input.SearchText) ||
               x.Email.Contains(input.SearchText) ||
               x.Id.ToString().Contains(input.SearchText) ||
               x.Endereco.Cidade.Contains(input.SearchText)
           );
    }

    public override async Task<PagedResultDto<ParceiroDto>> GetAllAsync(DefaultPagedDto input)
    {
        var pagedResultDto = await base.GetAllAsync(input);
        return pagedResultDto;
    }

    public List<ParceiroSimpleDto> GetAllParceirosAtivos()
    {
        IQueryable<Parceiro> parceiros = Repository.GetAll().Where(x => x.IsActive);
        List<ParceiroSimpleDto> parceiroSimpleDtos = ObjectMapper.Map<List<ParceiroSimpleDto>>(parceiros);
        return parceiroSimpleDtos;
    }

    public ParceiroSimpleDto GetByIdAsync(int id)
    {
        Parceiro parceiros = Repository.GetAll().FirstOrDefault(x => x.Id == id);
        ParceiroSimpleDto parceiroSimpleDtos = ObjectMapper.Map<ParceiroSimpleDto>(parceiros);
        return parceiroSimpleDtos;
    }



    protected override async Task<Parceiro> GetEntityByIdAsync(int id)
    {
        return Repository.GetAll()
            .Include(x => x.DiasPagamento)
            .Include(x => x.Endereco)
            .Include(x => x.ParceiroTabelasPreco).ThenInclude(x => x.TabelaPreco)
            .FirstOrDefault(x => x.Id == id);
    }

    public override Task<ParceiroDto> CreateAsync(ParceiroCreateDto input)
    {
        SetarEnderecoNulo(input);
        return base.CreateAsync(input);
    }

    public async override Task<ParceiroDto> UpdateAsync(ParceiroCreateDto input)
    {
        try
        {
            SetarEnderecoNulo(input);
            return await base.UpdateAsync(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void SetarEnderecoNulo(ParceiroCreateDto input)
    {
        if (input.Endereco.Cep == null && input.Endereco.Uf == null && input.Endereco.Cidade == null &&
           input.Endereco.Bairro == null && input.Endereco.Rua == null)
        {
            input.Endereco = null;
            input.EnderecoId = null;
        }
    }

    [AbpAuthorize(PermissionNames.Pages_Admin)]
    public override Task DeleteAsync(EntityDto<int> input)
    {
        return base.DeleteAsync(input);
    }

    public bool GetExistsCnpj(string cnpj)
    {
        var parceiros = Repository.GetAll().Where(x => x.Cnpj == cnpj);
        return parceiros.Any();

    }
}