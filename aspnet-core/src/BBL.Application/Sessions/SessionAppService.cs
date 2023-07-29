using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Uow;
using BBL.Sessions.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BBL.Sessions
{
    public class SessionAppService : BBLAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return output;
        }


        [HttpGet]
        [DisableAuditing]
        [AbpAuthorize]
        public async Task<Dictionary<string, string>> LoadPermissions()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                var allPermissionNames = PermissionManager.GetAllPermissions().Select(p => p.Name).ToList();
                var grantedPermissionNames = new List<string>();

                if (AbpSession.UserId.HasValue)
                {
                    foreach (var permissionName in allPermissionNames)
                    {
                        if (await PermissionChecker.IsGrantedAsync(permissionName))
                        {
                            grantedPermissionNames.Add(permissionName);
                        }
                    }
                }
                return grantedPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");

            }
        }
    }
}
