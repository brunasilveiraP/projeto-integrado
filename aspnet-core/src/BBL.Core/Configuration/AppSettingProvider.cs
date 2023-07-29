using System.Collections.Generic;
using Abp.Configuration;
using Abp.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace BBL.Configuration
{ public class AppSettingProvider : SettingProvider
    {
        private readonly IConfiguration configuration;

        public AppSettingProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(EmailSettingNames.DefaultFromAddress, configuration.GetValue("ConfiguracoesEnvioEmails:Remetente", string.Empty)),
                new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, configuration.GetValue("ConfiguracoesEnvioEmails:DefaultDisplayName", string.Empty)),
                new SettingDefinition(EmailSettingNames.Smtp.Host, configuration.GetValue("ConfiguracoesEnvioEmails:Servidor", string.Empty)),
                new SettingDefinition(EmailSettingNames.Smtp.Port, configuration.GetValue("ConfiguracoesEnvioEmails:Porta", string.Empty)),
                new SettingDefinition(EmailSettingNames.Smtp.UserName, configuration.GetValue("ConfiguracoesEnvioEmails:Usuario", string.Empty)),
                new SettingDefinition(EmailSettingNames.Smtp.Password, configuration.GetValue("ConfiguracoesEnvioEmails:Senha", string.Empty)),
                new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, configuration.GetValue("ConfiguracoesEnvioEmails:SSL", "false")),
                new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, configuration.GetValue("ConfiguracoesEnvioEmails:UsaCredenciaisPadrao", "false")),

                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, clientVisibilityProvider: new VisibleSettingClientVisibilityProvider())
            };
        }
    }
}
