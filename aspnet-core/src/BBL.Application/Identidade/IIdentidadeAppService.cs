using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BBL.Identidade.Dto;

namespace BBL.Identidade
{
    public interface IIdentidadeAppService : IApplicationService
    {
        Task<AcessarSistemaRetornoDto> CarregarIdentidade(string UserNameOrEmailAddress);
        Task AlterarSenha(AlterarSenhaDto dto);
        Task ExternalPasswordRecoveryAsync(string email);
        Task InternalPasswordRecoveryAsync(EntityDto<long> email);

    }
}