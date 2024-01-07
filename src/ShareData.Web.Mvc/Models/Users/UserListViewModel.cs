using System.Collections.Generic;
using ShareData.Roles.Dto;

namespace ShareData.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
