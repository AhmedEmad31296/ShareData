using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ShareData.Authorization
{
    public class ShareDataAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_DataForms, L("Form.DataForms"));
            context.CreatePermission(PermissionNames.Pages_WorkFlow, L("WorkFlow"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ShareDataConsts.LocalizationSourceName);
        }
    }
}
