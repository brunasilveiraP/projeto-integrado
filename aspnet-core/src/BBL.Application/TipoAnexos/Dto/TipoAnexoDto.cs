using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.TipoAnexos;

namespace BBL.TipoAnexos.Dto;

[AutoMap(typeof(TipoAnexo))]
public class TipoAnexoDto : EntityDto<int>
{
    public string Nome { get; set; }
}