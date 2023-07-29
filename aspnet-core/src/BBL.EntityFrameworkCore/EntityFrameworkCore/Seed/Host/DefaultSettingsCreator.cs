using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;

namespace BBL.EntityFrameworkCore.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly BblDbContext _context;

        public DefaultSettingsCreator(BblDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            if (BBLConsts.MultiTenancyEnabled == false)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "bruhngremio07@gmail.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "BBL Gestão", tenantId);

            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
