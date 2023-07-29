using Abp.Domain.Entities;
using BBL.Enums;
using BBL.Models.Enderecos;
using JetBrains.Annotations;

namespace BBL.Models.Clientes;

public class Cliente : Entity<int>
{
    public TipoPessoa TipoPessoa { get; set; }
    public string Cnpj { get; set; }
    public string Cpf { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int? EnderecoId { get; set; }
    public Endereco Endereco { get; set; }
}