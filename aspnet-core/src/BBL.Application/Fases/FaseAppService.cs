using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using BBL.Authorization;
using BBL.Fases.Dto;
using BBL.Helpers;
using BBL.Models.Fases;
using BBL.Models.Fases.Service;
using BBL.Models.Parceiros;
using BBL.Parceiros.Dto;

namespace BBL.Fases;

[AbpAuthorize]
public class FaseAppService : AsyncCrudAppService<Fase, FaseDto, int, DefaultPagedDto>
{
    private readonly IFaseService _faseService;
    public FaseAppService(
        IRepository<Fase, int> repository, IFaseService faseService) : base(repository)
    {
        _faseService = faseService;
    }

    protected override IQueryable<Fase> CreateFilteredQuery(DefaultPagedDto input)
    {
        return base.CreateFilteredQuery(input)
            .WhereIf(!string.IsNullOrEmpty(input.SearchText),
                x => x.Nome.Contains(input.SearchText) ||
                     x.Descricao.Contains(input.SearchText)
                );
    }
    
    [AbpAuthorize(PermissionNames.Pages_Admin)]
    public override Task DeleteAsync(EntityDto<int> input)
    {
        _faseService.CheckDelete(input.Id);
        return base.DeleteAsync(input);
    }

    [AbpAuthorize(PermissionNames.Pages_Visualizacao)]
    public List<FaseDto> GetAllFases()
    {
        IQueryable<Fase> fases = Repository.GetAll();
        List<FaseDto> faseDtos = ObjectMapper.Map<List<FaseDto>>(fases);
        return faseDtos;
    }
}