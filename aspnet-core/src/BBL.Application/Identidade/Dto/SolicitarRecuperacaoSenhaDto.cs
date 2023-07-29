using System.ComponentModel.DataAnnotations;

namespace BBL.Identidade.Dto
{
    public class SolicitarRecuperacaoSenhaDto
    {
        [Required]
        public string Email { get; set; }
    }
}
