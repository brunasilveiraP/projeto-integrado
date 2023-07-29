using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using BBL.Authorization;
using BBL.Helpers;
using BBL.Models.TabelaPrecos;
using BBL.Models.TabelaPrecos.Service;
using BBL.TabelaPrecos.Dto;

namespace BBL.TabelaPrecos;

[AbpAuthorize]
public class TabelaPrecoAppService : AsyncCrudAppService<TabelaPreco, TabelaPrecoDto, int, DefaultPagedDto>
{
    private readonly ITabelaPrecoService _tabelaPrecoService;
    public TabelaPrecoAppService(
        IRepository<TabelaPreco, int> repository,
        ITabelaPrecoService tabelaPrecoService
    ) : base(repository)
    {
        _tabelaPrecoService = tabelaPrecoService;
    }

    protected override IQueryable<TabelaPreco> CreateFilteredQuery(DefaultPagedDto input)
    {
        return base.CreateFilteredQuery(input)
            .WhereIf(!string.IsNullOrEmpty(input.SearchText),
                x=> x.KwpInicial.ToString().Contains(input.SearchText) ||
                    x.KwpFinal.ToString().Contains(input.SearchText)
                );
    }

    [AbpAuthorize(PermissionNames.Pages_Admin)]
    public override Task DeleteAsync(EntityDto<int> input)
    {
        _tabelaPrecoService.CheckDelete(input.Id);
        return base.DeleteAsync(input);
    }

    [AbpAuthorize(PermissionNames.Pages_Visualizacao)]
    public IEnumerable<TabelaPrecoSimpleDto> GetAllTabelasPrecoForEnum()
    {
        return ObjectMapper.Map<IEnumerable<TabelaPrecoSimpleDto>>(Repository.GetAll());
    }

}