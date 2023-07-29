using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Enums;
using BBL.Models.Anexos;
using BBL.TipoAnexos.Dto;

namespace BBL.Anexos.Dto;

[AutoMap(typeof(Anexo))]
public class AnexoDto : EntityDto<int>
{
    public byte[] Arquivo { get; set; }
    public string NomeArquivo { get; set; }
    public string TipoArquivo { get; set; }
    public int TipoAnexoId { get; set; }
    public TipoAnexoDto TipoAnexo { get; set; }
    public string Observacao { get; set; }
    public int ProjetoId { get; set; }
    public int TamanhoArquivo { get; set; }
}