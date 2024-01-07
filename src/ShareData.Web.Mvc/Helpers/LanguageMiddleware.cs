using Microsoft.AspNetCore.Http;

using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace ShareData.Web.Helpers
{
    public class LanguageMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var langCookie = context.Request.Cookies["Abp.Localization.CultureName"];
            var systemLanguage = "ar-eg"; // Default language if the cookie is not found.

            if (langCookie != null && langCookie.Contains("ar-eg"))
            {
                systemLanguage = langCookie;
            }
            else
            {
                context.Response.Cookies.Append("Abp.Localization.CultureName", systemLanguage, new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(2)
                });
            }

            var ci = new CultureInfo(systemLanguage);
            ci.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Saturday;
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.LongTimePattern = "hh:mm:ss";

            context.Response.Cookies.Append("Abp.Localization.CultureName", systemLanguage, new CookieOptions
            {
                Expires = DateTime.Now.AddYears(2)
            });


            Thread.CurrentThread.CurrentUICulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;

            Thread.CurrentThread.CurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentCulture = ci;

            // Call the next middleware component in the pipeline.
            await next(context);
        }
    }
}