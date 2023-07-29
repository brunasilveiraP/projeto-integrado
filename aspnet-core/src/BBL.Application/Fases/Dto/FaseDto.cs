using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.Fases;
using JetBrains.Annotations;

namespace BBL.Fases.Dto;

[AutoMap(typeof(Fase))]
public class FaseDto : EntityDto<int>
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public bool IncluiCobranca { get; set; }

}