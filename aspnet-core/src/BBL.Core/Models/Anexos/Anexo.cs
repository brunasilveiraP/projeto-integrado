using Abp.Domain.Entities.Auditing;
using BBL.Models.Parceiros;
using BBL.Models.Projetos;
using BBL.Models.TipoAnexos;
using JetBrains.Annotations;

namespace BBL.Models.Anexos;

public class Anexo : AuditedEntity<int>
{
    [CanBeNull] public byte[] Arquivo { get; set; }
    public string NomeArquivo { get; set; }
    public string TipoArquivo { get; set; }
    public int TamanhoArquivo { get; set; }
    public int TipoAnexoId { get; set; }
    public TipoAnexo TipoAnexo { get; set;}
    public Projeto Projeto { get; set; }
    public int ProjetoId { get; set; }
    
}