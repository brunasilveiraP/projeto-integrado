using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using BBL.Authorization.Roles;
using BBL.Authorization.Users;
using BBL.Configuration;
using BBL.Localization;
using BBL.Models.Parceiros;
using BBL.MultiTenancy;
using BBL.Timing;

namespace BBL
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class BBLCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Parceiro);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            BBLLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = BBLConsts.MultiTenancyEnabled;
            // Configuration.UnitOfWork.OverrideFilter(AbpDataFilters.MustHaveTenant, false);
            // Configuration.UnitOfWork.OverrideFilter(AbpDataFilters.MayHaveTenant, true);

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
            
            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));
            
            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = BBLConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = BBLConsts.DefaultPassPhrase;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BBLCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
