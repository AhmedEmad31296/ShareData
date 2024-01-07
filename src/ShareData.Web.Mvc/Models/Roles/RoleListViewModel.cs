using System.Collections.Generic;
using ShareData.Roles.Dto;

namespace ShareData.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
