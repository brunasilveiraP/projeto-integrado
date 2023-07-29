using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BBL.EntityFrameworkCore;
using BBL.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace BBL.Web.Tests
{
    [DependsOn(
        typeof(BBLWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class BBLWebTestModule : AbpModule
    {
        public BBLWebTestModule(BBLEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BBLWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(BBLWebMvcModule).Assembly);
        }
    }
}