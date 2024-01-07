using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using ShareData.Common;
using ShareData.Controllers;
using ShareData.DataForm;
using ShareData.DataForm.Dto;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShareData.Web.Controllers
{
    public class DataFormController : ShareDataControllerBase
    {
        public readonly IDataFormAppService _DataFormAppService;
        public DataFormController(IDataFormAppService dataFormAppService)
        {
            _DataFormAppService = dataFormAppService;
        }
        public IActionResult Index()
        {
            var dataSharingPeriodicities = Enum.GetValues(typeof(DataSharingPeriodicity)).Cast<DataSharingPeriodicity>().ToList();

            var dataSharingPeriodicityItems = dataSharingPeriodicities.Select(c => new SelectListItem
            {
                Value = ((int)c).ToString(),
                Text = L("Form.DataSharingPeriodicity." + c.ToString())
            }).ToList();

            ViewData["DataSharingPeriodicities"] = new SelectList(dataSharingPeriodicityItems, "Value", "Text");
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Create([FromBody] InsertDataFormInput input)
        {
            return Json(await _DataFormAppService.Insert(input));
        }

    }
}
