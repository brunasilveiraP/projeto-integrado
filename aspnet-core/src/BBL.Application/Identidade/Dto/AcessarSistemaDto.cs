using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using BBL.Validation;

namespace BBL.Identidade.Dto
{
    public class AcessarSistemaDto
    {
        [Required]
        [StringLength(NumeroDigitos.CPF)]
        public string Document { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string Password { get; set; }
    }
}
