using Abp.Application.Services;
using BBL.Fases.Dto;

namespace BBL.Fases;

public interface IFaseAppService : IAsyncCrudAppService<FaseDto>
{
    
}