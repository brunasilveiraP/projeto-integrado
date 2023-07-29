
using System.ComponentModel.DataAnnotations;

namespace BBL.Identidade.Dto
{
    public class AlterarSenhaDto
    {
        [Required]
        public string SenhaAtual { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[.#?!@$%^&*-])(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$")]
        public string NovaSenha { get; set; }
    }
}
