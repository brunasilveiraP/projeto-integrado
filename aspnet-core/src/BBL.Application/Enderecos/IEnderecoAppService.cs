using Abp.Application.Services;
using BBL.Enderecos.Dto;

namespace BBL.Enderecos;

public interface IEnderecoAppService : IAsyncCrudAppService<EnderecoDto>
{
    
}