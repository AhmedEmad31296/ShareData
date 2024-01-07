using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ShareData.Helpers;
using ShareData.Roles.Dto;

namespace ShareData.Roles
{
    public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>
    {
        Task<DatatableFilterdDto<RoleDto>> GetPaged(DatatableFilterInput input);

        Task<ListResultDto<PermissionDto>> GetAllPermissions();

        Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input);

        Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input);
    }
}
