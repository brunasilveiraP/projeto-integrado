using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using BBL.Authorization;
using BBL.Concessionarias.Dto;
using BBL.Helpers;
using BBL.Models.Concessionarias;
using BBL.Models.Concessionarias.Service;

namespace BBL.Concessionarias;

[AbpAuthorize]
public class ConcessionariaAppService : AsyncCrudAppService<Concessionaria, ConcessionariaDto, int, DefaultPagedDto>
{
    
    private readonly IConcessionariaService _concessionariaService;
    public ConcessionariaAppService(
        IRepository<Concessionaria, int> repository, IConcessionariaService concessionariaService) : base(repository)
    {
        _concessionariaService = concessionariaService;
    }

    protected override IQueryable<Concessionaria> CreateFilteredQuery(DefaultPagedDto input)
    {
        return base.CreateFilteredQuery(input)
            .WhereIf(!string.IsNullOrEmpty(input.SearchText), 
                x => x.Nome.Contains(input.SearchText) ||
                     x.Email.Contains(input.SearchText) ||
                     x.Telefone.Contains(input.SearchText));
    }

    [AbpAuthorize(PermissionNames.Pages_Admin)]
    public override Task DeleteAsync(EntityDto<int> input)
    {
        _concessionariaService.CheckDelete(input.Id);
        return base.DeleteAsync(input);
    }

    [AbpAuthorize(PermissionNames.Pages_Visualizacao)]
    public List<ConcessionariaDto> GetAllConcessionarias()
    {
        IQueryable<Concessionaria> Concessionarias = Repository.GetAll();
        List<ConcessionariaDto> ConcessionariaDtos = ObjectMapper.Map<List<ConcessionariaDto>>(Concessionarias);
        return ConcessionariaDtos;
    }
}