using Abp.Application.Services;
using BBL.Clientes.Dto;
using BBL.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BBL.Clientes;

public interface IClienteAppService : IAsyncCrudAppService<CreateClienteDto>
{
    
}