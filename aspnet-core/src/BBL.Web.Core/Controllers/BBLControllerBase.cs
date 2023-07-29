using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace BBL.Controllers
{
    public abstract class BBLControllerBase: AbpController
    {
        protected BBLControllerBase()
        {
            LocalizationSourceName = BBLConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
