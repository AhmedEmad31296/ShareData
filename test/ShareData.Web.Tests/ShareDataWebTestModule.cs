using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ShareData.EntityFrameworkCore;
using ShareData.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ShareData.Web.Tests
{
    [DependsOn(
        typeof(ShareDataWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class ShareDataWebTestModule : AbpModule
    {
        public ShareDataWebTestModule(ShareDataEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ShareDataWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ShareDataWebMvcModule).Assembly);
        }
    }
}