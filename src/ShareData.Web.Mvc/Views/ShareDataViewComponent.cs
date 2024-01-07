using Abp.AspNetCore.Mvc.ViewComponents;

namespace ShareData.Web.Views
{
    public abstract class ShareDataViewComponent : AbpViewComponent
    {
        protected ShareDataViewComponent()
        {
            LocalizationSourceName = ShareDataConsts.LocalizationSourceName;
        }
    }
}
