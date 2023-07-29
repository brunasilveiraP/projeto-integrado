using Abp.Application.Services;
using BBL.Parceiros.Dto;

namespace BBL.Parceiros;

public interface IParceiroAppService : IAsyncCrudAppService<ParceiroDto>
{
    
}