using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Net.Mail;
using Abp.UI;
using BBL.Authorization.Gerenciadores;
using BBL.Authorization.Users;
using BBL.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BBL.Identidade
{
    public class IdentidadeService : DomainService, IIdentidadeService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<User, long> _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public IdentidadeService(IPasswordHasher<User> passwordHasher, IRepository<User, long> userRepository, IEmailSender emailSender, IConfiguration configuration)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task SendRecoveryPasswordAsync(User usuario)
        {
            var password = await UpdateNewPassword(usuario);
            await SendEmailPassworAsync(
                usuario,
                Properties.Resources.recoveryPasswordSubject,
                string.Format(Properties.Resources.recoveryPasswordBody, usuario.Name, password));
        }

        public async Task SendNewUserPasswordAsync(User usuario)
        {
            var clientAddress = _configuration.GetValue("App:ClientRootAddress", string.Empty);
            var password = await UpdateNewPassword(usuario);
            await SendEmailPassworAsync(
                usuario,
                Properties.Resources.newUserPasswordSubject,
                string.Format(Properties.Resources.newUserPasswordBody, usuario.Name, usuario.UserName, password, clientAddress));
        }

        private async Task<string> UpdateNewPassword(User usuario)
        {
            string senhaTemporaria = GerenciadorSenhas.GerarSenhaTemporaria();
            string senhaCriptografada = _passwordHasher.HashPassword(usuario, senhaTemporaria);

            usuario.AtribuirSenhaTemporaria(senhaCriptografada);
            await _userRepository.UpdateAsync(usuario);

            return senhaTemporaria;
        }

        public async Task SendEmailPassworAsync(User usuario, string subject, string body)
        {
            if (usuario.EmailAddress != null)
            {
                try
                {
                    await _emailSender.SendAsync(
                         to: usuario.EmailAddress,
                         subject: subject,
                         body: body,
                         isBodyHtml: true);
                }
                catch (Exception ex)
                {
                    Logger.Error("Erro ao tentar enviar e-mail.", ex);
                }
            }
            else
                throw new UserFriendlyException("Usuário não possui e-mail cadastrado.");
        }
    }
}
