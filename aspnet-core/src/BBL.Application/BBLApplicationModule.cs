using BBL.Exceptions;
using Abp.AspNetCore.Mvc.ExceptionHandling;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BBL.Authorization;
using System;
using Abp.Threading.BackgroundWorkers;
using Abp.Web.Models;
using BBL.Exceptions.Converters;

namespace BBL
{
    [DependsOn(
        typeof(BBLCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class BBLApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<BBLAuthorizationProvider>();

            Configuration.ReplaceService<AbpExceptionFilter, GlobalExceptionFilter>(Abp.Dependency.DependencyLifeStyle.Transient);

        }

        public override void Initialize()
        {
            var thisAssembly = typeof(BBLApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }

        public override void PostInitialize()
        {
            var errorInfoBuilder = IocManager.Resolve<IErrorInfoBuilder>();
            errorInfoBuilder.AddExceptionConverter(new SqlExceptionToErrorInfoConverter());

            base.PostInitialize();
        }

        public class MyExceptionHandler : IEventHandler<AbpHandledExceptionData>, ITransientDependency
        {
            public void HandleEvent(AbpHandledExceptionData eventData)
            {                
            }
        }
    }
}
