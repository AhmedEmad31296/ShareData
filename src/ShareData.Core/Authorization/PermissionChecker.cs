using Abp.Authorization;
using ShareData.Authorization.Roles;
using ShareData.Authorization.Users;

namespace ShareData.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
