using System.Linq;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using BBL.Clientes.Dto;
using BBL.Helpers;
using BBL.Models.Clientes;
using Microsoft.EntityFrameworkCore;

namespace BBL.Clientes;

public class ClienteAppService : AsyncCrudAppService<Cliente, CreateClienteDto, int, DefaultPagedDto>
{
    public ClienteAppService(IRepository<Cliente, int> repository) : base(repository)
    {
    }

    protected override IQueryable<Cliente> CreateFilteredQuery(DefaultPagedDto input)
    {
        return base.CreateFilteredQuery(input)
            .Include(x=> x.Endereco);
    }

}