using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.Parceiros;
using BBL.MultiTenancy;

namespace BBL.Sessions.Dto
{
    [AutoMapFrom(typeof(Parceiro))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
