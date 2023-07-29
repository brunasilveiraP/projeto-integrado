using System.Threading.Tasks;
using Abp.Domain.Services;
using BBL.Authorization.Users;

namespace BBL.Identidade
{
    public interface IIdentidadeService : IDomainService
    {
        Task SendRecoveryPasswordAsync(User usuario);
        Task SendNewUserPasswordAsync(User usuario);

        Task SendEmailPassworAsync(User usuario, string subject, string body);
    }
}