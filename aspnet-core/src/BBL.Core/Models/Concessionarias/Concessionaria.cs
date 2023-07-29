using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

namespace BBL.Models.Concessionarias;

public class Concessionaria : AuditedEntity<int>
{
    public string Nome { get; set; }
    [CanBeNull] public string Telefone { get; set; }
    [CanBeNull] public string Email { get; set; }
}