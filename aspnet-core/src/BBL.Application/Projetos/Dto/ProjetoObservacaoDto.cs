using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.Projetos;

namespace BBL.Projetos.Dto
{
    [AutoMap(typeof(Observacao))]
    public class ProjetoObservacaoDto : EntityDto<int>
    {
        public int ProjetoId { get; set; }
        public string Descricao { get; set; }
        public bool Fixed { get; set; }
    }
}
