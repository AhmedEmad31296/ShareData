using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ShareData.Authorization;

namespace ShareData
{
    [DependsOn(
        typeof(ShareDataCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ShareDataApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ShareDataAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ShareDataApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
