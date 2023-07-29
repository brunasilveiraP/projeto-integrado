using Abp.Authorization;
using Abp.Authorization.Roles;
using BBL.Authorization.Roles;
using BBL.Models.Fases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.EntityFrameworkCore.Seed.Perfil
{
    public class PerfilCreator
    {
        private readonly BblDbContext _context;

        public PerfilCreator(BblDbContext context)
        {
            _context = context;
        }

        public async void Create()
        {
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                var registros = _context.Roles.Count();
                PermissionSetting permissionSetting = _context.Permissions.FirstOrDefault(x => x.Name == "Pages.Visualizacao");



                if (registros == 1)
                {
                    _context.Roles.AddRange(new List<Role>
                    {
                        new Role
                        {
                            Name = "Parceiro",
                            DisplayName = "Parceiro",
                            NormalizedName = "Parceiro",
                        },
                    });
                    _context.SaveChanges();

                    Role role = _context.Roles.FirstOrDefault(x => x.Name == "Parceiro");
                    if (role != null)
                    {
                        RolePermissionSetting rolePermissionSetting = new()
                        {
                            Name = permissionSetting.Name,
                            IsGranted = permissionSetting.IsGranted,
                            RoleId = role.Id
                        };
                        _context.SaveChanges();
                    }
                }

            }
        }
        
    }
}
