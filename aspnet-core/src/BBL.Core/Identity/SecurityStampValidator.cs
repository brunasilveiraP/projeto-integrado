using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using BBL.Authorization.Roles;
using BBL.Authorization.Users;
using BBL.MultiTenancy;
using Microsoft.Extensions.Logging;
using Abp.Domain.Uow;
using BBL.Models.Parceiros;

namespace BBL.Identity
{
    public class SecurityStampValidator : AbpSecurityStampValidator<Parceiro, Role, User>
    {
        public SecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options,
            SignInManager signInManager,
            ISystemClock systemClock,
            ILoggerFactory loggerFactory,
            IUnitOfWorkManager unitOfWorkManager)
            : base(options, signInManager, systemClock, loggerFactory, unitOfWorkManager)
        {
        }
    }
}
