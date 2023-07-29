using System.Linq;
using AutoMapper;
using BBL.Authorization.Users;

namespace BBL.Users.Dto
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<UserDto, User>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<User, UserDto>()
                .ForMember(x => x.TenantName, 
                    opt => opt.MapFrom(x => x.Tenant.Fantasia))
                ;            
            CreateMap<User, UserSimpleDto>()
                .ForMember(x => x.Login, 
                    opt => opt.MapFrom(x => x.UserName))
                .ForMember(x => x.Email, 
                    opt => opt.MapFrom(x => x.EmailAddress))
                .ForMember(x => x.Nome, 
                    opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.Parceiro, 
                    opt => opt.MapFrom(x => x.Tenant.Fantasia))
                ;
            
            CreateMap<UserSimpleDto, User>()
                .ForMember(x => x.UserName, 
                    opt => opt.MapFrom(x => x.Login))
                .ForMember(x => x.EmailAddress, 
                    opt => opt.MapFrom(x => x.Email))
                .ForMember(x => x.Name, 
                    opt => opt.MapFrom(x => x.Nome))
                ;

            CreateMap<CreateUserDto, User>();
            CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());
        }
    }
}
