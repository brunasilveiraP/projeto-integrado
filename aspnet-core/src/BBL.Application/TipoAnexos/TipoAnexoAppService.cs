using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using BBL.Authorization;
using BBL.Helpers;
using BBL.Models.TipoAnexos;
using BBL.Models.TipoAnexos.Service;
using BBL.TipoAnexos.Dto;

namespace BBL.TipoAnexos;

[AbpAuthorize]
public class TipoAnexoAppService : AsyncCrudAppService<TipoAnexo, TipoAnexoDto, int, DefaultPagedDto>
{
    private readonly ITipoAnexoService _tipoAnexoService;
    public TipoAnexoAppService(
        IRepository<TipoAnexo, int> repository, ITipoAnexoService tipoAnexoService) : base(repository)
    {
        _tipoAnexoService = tipoAnexoService;
    }

    protected override IQueryable<TipoAnexo> CreateFilteredQuery(DefaultPagedDto input)
    {
        return base.CreateFilteredQuery(input)
            .WhereIf(!string.IsNullOrEmpty(input.SearchText), 
                x => x.Nome.Contains(input.SearchText));
    }

    [AbpAuthorize(PermissionNames.Pages_Admin)]
    public override Task DeleteAsync(EntityDto<int> input)
    {
        _tipoAnexoService.CheckDelete(input.Id);
        return base.DeleteAsync(input);
    }

    [AbpAuthorize(PermissionNames.Pages_Visualizacao)]
    public override Task<PagedResultDto<TipoAnexoDto>> GetAllAsync(DefaultPagedDto input)
    {
        return base.GetAllAsync(input);
    }
}