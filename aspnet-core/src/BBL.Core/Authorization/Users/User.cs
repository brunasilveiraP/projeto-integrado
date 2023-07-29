using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using BBL.Models.Parceiros;

namespace BBL.Authorization.Users
{
    public class User : AbpUser<User>
    {
        
        public const string DefaultPassword = "123qwe";

        public Parceiro Tenant { get; set; }
        
        public bool CheckedToChangePassword { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
        
        public void AtribuirSenhaTemporaria(string senhaCriptografada)
        {
            Password = senhaCriptografada;
            CheckedToChangePassword = true;
        }  
    }
}
