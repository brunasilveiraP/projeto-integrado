using System.Collections.Generic;
using Abp.Domain.Entities;
using BBL.Enums;
using BBL.Models.TabelaPrecos;

namespace BBL.Models.Parceiros;

public class ParceiroTabelaPreco : Entity<int>
{
    public Parceiro Parceiro { get; set; }
    public int ParceiroId { get; set; }
    public TabelaPreco TabelaPreco { get; set; }
    public int TabelaPrecoId { get; set; }

    public ParceiroTabelaPreco(int parceiroId, int tabelaPrecoId)
    {
        ParceiroId = parceiroId;
        TabelaPrecoId = tabelaPrecoId;
    }
}