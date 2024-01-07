using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.Common
{
    public static class Helpers
    {
        public static string GetAction(this IUrlHelper html, string action)
        {
            string _url = "/";
            if (html.ActionContext.HttpContext.GetRouteValue("area") != null)
                _url += html.ActionContext.HttpContext.GetRouteValue("area").ToString() + "/";
            _url += html.ActionContext.HttpContext.GetRouteValue("controller").ToString() + "/";
            _url += action;
            return _url;
        }
    }
}
