using Abp.Application.Services;
using BBL.Concessionarias.Dto;

namespace BBL.Cobrancas;

public interface ICobrancaAppService : IAsyncCrudAppService<ConcessionariaDto>
{
    
}