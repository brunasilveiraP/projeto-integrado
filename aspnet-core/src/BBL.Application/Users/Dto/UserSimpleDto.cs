using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Authorization.Users;
using BBL.Parceiros.Dto;

namespace BBL.Users.Dto
{
    [AutoMap(typeof(User))]
    public class UserSimpleDto : EntityDto<long>
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        public long ParceiroId { get; set; }
        public ParceiroSimpleDto Parceiro { get; set; }
        
        public string[] RoleNames { get; set; }
        
    }
}
