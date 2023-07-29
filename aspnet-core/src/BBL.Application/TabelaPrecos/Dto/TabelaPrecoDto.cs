using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.TabelaPrecos;

namespace BBL.TabelaPrecos.Dto;

[AutoMap(typeof(TabelaPreco))]
public class TabelaPrecoDto : EntityDto<int>
{
    public uint KwpInicial { get; set; }
    public uint KwpFinal { get; set; }
    public string Nome { get; set; }
    public double Valor { get; set; }
}