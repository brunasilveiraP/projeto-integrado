using Abp.Domain.Entities.Auditing;

namespace BBL.Models.Fases;

public class Fase : AuditedEntity<int>
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public bool  IncluiCobranca { get; set; }

}