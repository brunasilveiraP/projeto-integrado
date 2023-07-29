using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace BBL.Models.Parceiros;

public class DiaPagamento : Entity<int>
{
    public Parceiro Parceiro { get; set; }
    public int ParceiroId { get; set; }
    [MaxLength(31)]
    public uint Dia { get; set; }

    public DiaPagamento(uint dia)
    {
        Dia = dia;
    }
}