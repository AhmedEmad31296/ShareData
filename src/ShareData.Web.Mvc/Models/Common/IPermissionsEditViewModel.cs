using System.Collections.Generic;
using ShareData.Roles.Dto;

namespace ShareData.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}