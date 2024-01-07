using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using ShareData.Authorization;
using ShareData.Controllers;
using ShareData.Users;
using ShareData.Web.Models.Users;
using System;
using System.Linq;
using ShareData.Users.Dto;
using ShareData.Helpers;

namespace ShareData.Web.Controllers
{
    [AbpMvcAuthorize]
    public class UsersController : ShareDataControllerBase
    {
        private readonly IUserAppService _userAppService;
        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }
        public async Task<ActionResult> Index()
        {
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new UserListViewModel
            {
                Roles = roles
            };
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> GetPaged()
        {
            DatatableFilterInput input = GetDatatableFilterInput();
            DatatableFilterdDto<UserDto> result = await _userAppService.GetPaged(input);
            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> EditModal(long id)
        {
            var user = await _userAppService.GetAsync(new EntityDto<long>(id));
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new EditUserModalViewModel
            {
                User = user,
                Roles = roles
            };
            return PartialView("_EditModal", model);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Create([FromBody] CreateUserDto input)
        {
            var output = await _userAppService.CreateAsync(input);
            return Json(output);
        }
        [HttpPost]
        public async Task<JsonResult> Update([FromBody] UserDto input)
        {
            return Json(await _userAppService.UpdateAsync(input));
        }
        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            var input = new EntityDto<long> { Id = id };
            try
            {
                await _userAppService.DeleteAsync(input);
                return Json(new { success = true, message = L("DeletedSuccessfully") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = L("UserDeleteWarningMessage") + ex.Message });
            }
        }
    }
}
