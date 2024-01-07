using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using ShareData.Configuration.Dto;

namespace ShareData.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ShareDataAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
