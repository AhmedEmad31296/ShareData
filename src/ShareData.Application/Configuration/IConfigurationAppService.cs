using System.Threading.Tasks;
using ShareData.Configuration.Dto;

namespace ShareData.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
