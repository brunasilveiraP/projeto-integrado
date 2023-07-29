using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.Parceiros;

namespace BBL.Parceiros.Dto;

[AutoMap(typeof(ParceiroTabelaPreco))]
public class ParceiroTabelaPrecoDto : EntityDto<int>
{
    public int ParceiroId { get; set; }
    public int TabelaPrecoId{ get; set; }
}