using System.Threading.Tasks;
using BBL.Configuration.Dto;

namespace BBL.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
