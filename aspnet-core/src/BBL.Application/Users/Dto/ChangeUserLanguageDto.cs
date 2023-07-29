using System.ComponentModel.DataAnnotations;

namespace BBL.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}