using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ShareData.Web.Views
{
    public abstract class ShareDataRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ShareDataRazorPage()
        {
            LocalizationSourceName = ShareDataConsts.LocalizationSourceName;
        }
    }
}
