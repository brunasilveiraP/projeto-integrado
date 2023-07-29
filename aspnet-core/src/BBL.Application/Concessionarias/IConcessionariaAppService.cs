using Abp.Application.Services;
using BBL.Concessionarias.Dto;

namespace BBL.Concessionarias;

public interface IConcessionariaAppService : IAsyncCrudAppService<ConcessionariaDto>
{
    
}