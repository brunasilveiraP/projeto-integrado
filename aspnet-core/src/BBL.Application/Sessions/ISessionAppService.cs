using System.Threading.Tasks;
using Abp.Application.Services;
using BBL.Sessions.Dto;

namespace BBL.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
