using Abp.Application.Services;
using BBL.TabelaPrecos.Dto;

namespace BBL.TabelaPrecos;

public interface ITabelaPrecoAppService : IAsyncCrudAppService<TabelaPrecoDto>
{
    
}