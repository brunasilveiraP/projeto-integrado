using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using BBL.Authorization;
using BBL.Authorization.Users;
using BBL.Identidade.Dto;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace BBL.Identidade
{
    public class IdentidadeAppService :
           ApplicationService, IIdentidadeAppService
    {
        private readonly IRepository<User, long> _repository;
        private readonly UserManager _userManager;
        private readonly LogInManager _logInManager;
        private readonly IAbpSession _abpSession;
        private readonly IIdentidadeService _identidadeService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public IdentidadeAppService(
            IRepository<User, long> repository,
            IAbpSession abpSession,
            UserManager userManager,
            LogInManager logInManager,
            IdentidadeService identidadeService,
            IPasswordHasher<User> passwordHasher,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _repository = repository;
            _userManager = userManager;
            _logInManager = logInManager;
            _abpSession = abpSession;
            _identidadeService = identidadeService;
            _passwordHasher = passwordHasher;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [RemoteService(false)]
        public async Task<AcessarSistemaRetornoDto> CarregarIdentidade(string UserNameOrEmailAddress)
        {
            var usuario =
                 _repository.GetAll()
                        .Where(x => x.EmailAddress.Equals(UserNameOrEmailAddress) || x.UserName.Equals(UserNameOrEmailAddress) && x.IsActive).FirstOrDefault();

            if (usuario == null)
            {
                throw new UserFriendlyException("Nome de usuário  inválido.");
            }
            return ObjectMapper.Map<AcessarSistemaRetornoDto>(usuario);
        }

        [AbpAuthorize]
        public async Task AlterarSenha(AlterarSenhaDto dto)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException(
                    "É preciso estar autenticado para realizar esta operação.");
            }

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
            {

                var usuario = await _userManager.GetUserByIdAsync(_abpSession.UserId.Value);

                var loginAsync =
                    await _logInManager.LoginAsync(
                        usuario.UserName, dto.SenhaAtual, shouldLockout: false);

                if (loginAsync.Result != AbpLoginResultType.Success)
                {
                    throw new UserFriendlyException("Usuário ou senha inválidos.");
                }

                usuario.AtribuirSenhaTemporaria(_passwordHasher.HashPassword(usuario, dto.NovaSenha));
                usuario.CheckedToChangePassword = false;

                await _repository.UpdateAsync(usuario);
            }

        }

        [AbpAuthorize]
        public async Task InternalPasswordRecoveryAsync(EntityDto<long> entity)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
            {
                var user = await _repository.GetAsync(entity.Id);

                if (user.IsNullOrDeleted())
                {
                    throw new UserFriendlyException("E-mail informado está incorreto ou não está cadastrado.");
                }
                Logger.Error("Erro ao tentar enviar email de Recuperação de Senha.", null);

                await _identidadeService.SendRecoveryPasswordAsync(user);
            }
        }

        public async Task ExternalPasswordRecoveryAsync(string email)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
            {
                var user = _repository.GetAll()
                .Where(x => x.EmailAddress.Equals(email) && x.IsActive).FirstOrDefault();

                if (user.IsNullOrDeleted())
                {
                    throw new UserFriendlyException("E-mail informado está incorreto ou não está cadastrado.");
                }
                Logger.Error("Erro ao tentar enviar email de Recuperação de Senha.", null);

                await _identidadeService.SendRecoveryPasswordAsync(user);
            }
        }

    }

}
