using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace BBL.Authorization
{
    public class BBLAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"));

            context.CreatePermission(PermissionNames.Pages_Admin, L("Admin"),
                    L(@"Funcionalidade que permite o gerenciamento total do sistema."));

            context.CreatePermission(PermissionNames.Pages_Visualizacao, L("Visualizacao"),
                    L(@"Funcionalidade que permite o parceiro visualizar vinculos."));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BBLConsts.LocalizationSourceName);
        }
    }
}
