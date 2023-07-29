using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using BBL.Authorization.Users;
using BBL.Editions;
using BBL.Models.Parceiros;

namespace BBL.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Parceiro, User>
    {
        public TenantManager(
            IRepository<Parceiro> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore)
        {
        }
    }
}
