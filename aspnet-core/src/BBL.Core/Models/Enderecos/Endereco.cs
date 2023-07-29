using Abp.Domain.Entities.Auditing;
using BBL.Models.Clientes;
using BBL.Models.Parceiros;
using JetBrains.Annotations;

namespace BBL.Models.Enderecos;

public class Endereco : AuditedEntity<int>
{
    [CanBeNull] public string Cep { get; set; }
    public string Uf { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public string Numero { get; set; }
    [CanBeNull] public string Complemento { get; set; }

    public int? ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    public Parceiro Parceiro { get; set; }
    public int? ParceiroId { get; set; }
}