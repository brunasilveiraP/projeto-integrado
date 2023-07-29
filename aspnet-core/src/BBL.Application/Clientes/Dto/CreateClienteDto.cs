using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Enderecos.Dto;
using BBL.Enums;
using BBL.Models.Clientes;
using JetBrains.Annotations;

namespace BBL.Clientes.Dto;

[AutoMap(typeof(Cliente))]
public class CreateClienteDto : EntityDto<int>
{
    public TipoPessoa TipoPessoa { get; set; }
    public string Cnpj { get; set; }
    public string Cpf { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int? EnderecoId { get; set; }
    public EnderecoDto Endereco { get; set; }
    
}