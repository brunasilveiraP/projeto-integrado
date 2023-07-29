using Abp.Domain.Entities.Auditing;
using BBL.Authorization.Users;

namespace BBL.Models.Projetos
{
    public class Observacao : AuditedEntity<int>
    {
        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; }
        public string Descricao { get; set; }
        public bool Fixed { get; set; }
    }
}
