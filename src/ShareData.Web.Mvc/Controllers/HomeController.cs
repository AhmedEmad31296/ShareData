using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using ShareData.Controllers;

namespace ShareData.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : ShareDataControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
