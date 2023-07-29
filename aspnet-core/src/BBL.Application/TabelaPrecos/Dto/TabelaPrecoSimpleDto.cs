using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.TabelaPrecos;

namespace BBL.TabelaPrecos.Dto;

[AutoMap(typeof(TabelaPreco))]
public class TabelaPrecoSimpleDto : EntityDto<int>
{
    public string Descricao { get; set; }
}