using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.Parceiros;

namespace BBL.Parceiros.Dto;

[AutoMap(typeof(Parceiro))]
public class ParceiroSimpleDto : EntityDto<int>
{
    public string Fantasia { get; set; }
}