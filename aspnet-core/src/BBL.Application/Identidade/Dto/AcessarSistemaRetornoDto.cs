using Abp.AutoMapper;
using BBL.Authorization.Users;

namespace BBL.Identidade.Dto
{
    [AutoMap(typeof(User))]
    public class AcessarSistemaRetornoDto
    {
        public string User { get; set; }

        public bool CheckedToChangePassword { get; set; }

        public bool IsActive { get; set; }
    }
}
