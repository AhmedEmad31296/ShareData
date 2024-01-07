using Abp.AspNetCore.Mvc.Authorization;
using Abp.Runtime.Session;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Linq;
using System.Threading.Tasks;

using ShareData.Authorization;
using ShareData.Controllers;
using ShareData.Helpers;
using ShareData.Users;
using ShareData.DataForm;
using ShareData.DataForm.Dto;
using ShareData.Common;

namespace ShareData.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_DataForms)]
    public class DataFormStageController : ShareDataControllerBase
    {
        private readonly IDataFormAppService _DataFormAppService;
        public DataFormStageController(IDataFormAppService dataFormAppService)
        {
            _DataFormAppService = dataFormAppService;
        }
        public async Task<ActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var dataForm = _DataFormAppService.Get(id);
            var dataSharingPeriodicities = Enum.GetValues(typeof(DataSharingPeriodicity)).Cast<DataSharingPeriodicity>().ToList();
            var dataSharingPeriodicityItems = dataSharingPeriodicities.Select(c => new SelectListItem
            {
                Value = ((int)c).ToString(),
                Text = L("Form.DataSharingPeriodicity." + c.ToString())
            }).ToList();

            ViewData["DataSharingPeriodicities"] = new SelectList(dataSharingPeriodicityItems, "Value", "Text", dataForm.DataSharingPeriodicity);

            return View(dataForm);
        }
        [HttpGet]
        public async Task<ActionResult> GetNextWorkFlowStageDetailsForDataForm(int id, LevelStatus stageType)
        {
            var nextStage = await _DataFormAppService.GetNextWorkFlowStageDetailsForDataForm(id, stageType);
            return PartialView("_SendFormNoteModal", nextStage);
        }
        [HttpPost]
        public async Task<JsonResult> GetPaged()
        {
            int draw = Convert.ToInt32(HttpContext.Request.Form["draw"].FirstOrDefault());
            int start = Convert.ToInt32(HttpContext.Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(HttpContext.Request.Form["length"].FirstOrDefault());
            string sortColumn = HttpContext.Request.Form[$"columns[{HttpContext.Request.Form["order[0][column]"].FirstOrDefault()}][name]"].FirstOrDefault();
            string sortColumnDir = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            string searchTerm = HttpContext.Request.Form["search[value]"].FirstOrDefault();

            FilterDataFormPagedInput input = new()
            {
                Draw = draw,
                Page = start / length + 1,
                PageSize = length,
                SortColumn = sortColumn,
                SortDirection = sortColumnDir,
                SearchTerm = searchTerm
            };

            DatatableFilterdDto<DataFormPagedDto> result = await _DataFormAppService.GetPaged(input);
            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            return Json(await _DataFormAppService.Delete(id));
        }
        [HttpPost]
        public async Task<JsonResult> InsertNewFormStage(InsertNewFormStageInput input)
        {
            return Json(await _DataFormAppService.InsertNewFormStage(input));
        }
        
    }
}
