using System.Threading.Tasks;
using Abp.Application.Services;
using BBL.Authorization.Accounts.Dto;

namespace BBL.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
