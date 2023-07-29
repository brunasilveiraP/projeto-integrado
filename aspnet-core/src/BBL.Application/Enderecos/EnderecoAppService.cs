using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using BBL.Enderecos.Dto;
using BBL.Models.Enderecos;

namespace BBL.Enderecos;

public class EnderecoAppService : AsyncCrudAppService<Endereco, EnderecoDto, int>
{
    public EnderecoAppService(IRepository<Endereco, int> repository) : base(repository)
    {
    }

}