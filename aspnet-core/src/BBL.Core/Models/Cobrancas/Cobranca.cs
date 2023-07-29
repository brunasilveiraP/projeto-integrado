using System;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using BBL.Enums;
using BBL.Models.Parceiros;
using BBL.Models.Projetos;

namespace BBL.Models.Cobrancas;

public class Cobranca : AuditedEntity<int>
{
    public int TenantId { get; set; }
    public Parceiro Tenant { get; set; }
    public double ValorTotal { get; set; }
    public DateTime? DataPagamento { get; set; }
    public DateTime? DataGeracao { get; set; }
    public StatusPagamento StatusPagamento { get; set; }
    public ICollection<Projeto> Projetos { get; set; }
}