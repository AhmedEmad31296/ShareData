using System.Collections.Generic;
using System.Threading.Tasks;

using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ShareData.Authorization.Users;
using ShareData.Helpers;
using ShareData.Roles.Dto;
using ShareData.Users.Dto;

namespace ShareData.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<DatatableFilterdDto<UserDto>> GetPaged(DatatableFilterInput input);
        Task DeActivate(EntityDto<long> user);
        Task Activate(EntityDto<long> user);
        Task<ListResultDto<RoleDto>> GetRoles();
        Task ChangeLanguage(ChangeUserLanguageDto input);
        Task<List<User>> GetUsersByRoleIdAsync(int roleId);
        Task<bool> ChangePassword(ChangePasswordDto input);
    }
}
