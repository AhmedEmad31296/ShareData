using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using ShareData.Authorization.Roles;
using ShareData.Authorization.Users;
using ShareData.Configuration;
using ShareData.Localization;
using ShareData.MultiTenancy;
using ShareData.Timing;

namespace ShareData
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class ShareDataCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            ShareDataLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = ShareDataConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.Localization.Languages.Add(new LanguageInfo("en", "En", "famfamfam-flags en"));
            Configuration.Localization.Languages.Add(new LanguageInfo("ar-eg", "العربية", "famfamfam-flags ar", true));

            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = ShareDataConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = ShareDataConsts.DefaultPassPhrase;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ShareDataCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
