using Abp.Authorization;
using BBL.Authorization.Roles;
using BBL.Authorization.Users;

namespace BBL.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
