using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using BBL.Configuration.Dto;

namespace BBL.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : BBLAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
