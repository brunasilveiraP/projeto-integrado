using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using BBL.Models.Parceiros;

namespace BBL.Models.TabelaPrecos;

public class TabelaPreco : AuditedEntity<int>
{
    public uint KwpInicial { get; set; }
    public uint KwpFinal { get; set; }
    public double Valor { get; set; }
    
    public IList<ParceiroTabelaPreco> ParceiroTabelasPreco { get; set; }
}