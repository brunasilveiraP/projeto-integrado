using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Net.Mail;
using Abp.UI;
using BBL.Authorization.Users;
using BBL.Enums;
using Microsoft.EntityFrameworkCore;

namespace BBL.Models.Projetos.Service;

public class ProjetoService : DomainService, IProjetoService
{
    private readonly IRepository<Projeto, int> _repository;
    private readonly IEmailSender _emailSender;
    private readonly IRepository<User, long> _userRepository;

    public ProjetoService(IRepository<Projeto, int> repository, IEmailSender emailSender, IRepository<User, long> userRepository)
    {
        _repository = repository;
        _emailSender = emailSender;
        _userRepository = userRepository;
    }

    public void CalcularValorProjeto(int projetoId)
    {
        var projeto = _repository.GetAll()
            .Include(x => x.Tenant)
                .ThenInclude(x => x.ParceiroTabelasPreco)
                .ThenInclude(x => x.TabelaPreco).Where(x => x.Id == projetoId)
            .FirstOrDefault();
        
        double valorProjeto = 0;
            var tabelaPreco = projeto.Tenant.ParceiroTabelasPreco
                .Where(x =>
                    ((projeto.PotenciaModulo * projeto.QuantidadeModulo) / 1000) >= x.TabelaPreco.KwpInicial &&
                    ((projeto.PotenciaModulo * projeto.QuantidadeModulo) / 1000) <= x.TabelaPreco.KwpFinal)
                .Select(x => x.TabelaPreco).FirstOrDefault();

            if (tabelaPreco == null)
            {
                throw new UserFriendlyException(
                    "Nenhuma tabela de pre�o com a quantidade de KWP foi vinculada ao Parceiro. Verificar cadastro do parceiro e posteriormente salvar o projeto novamente.");
            }
                
            valorProjeto = ((projeto.PotenciaModulo * projeto.QuantidadeModulo) / 1000) * tabelaPreco.Valor;

            if (projeto.Tenant.PagaTrt == SimNao.Nao) valorProjeto += 65;

            projeto.Valor= valorProjeto;
            _repository.Update(projeto);
    }
    
    public void NotificarCriacaoProjeto(int projetoId)
    {
        Projeto projeto = _repository.GetAll()
            .Include(x => x.Tenant)
            .FirstOrDefault(x => x.Id == projetoId);
        
        List<User> admins = _userRepository.GetAll().Where( x => x.Roles.Select(x => x.RoleId).Contains(1)).ToList();

        foreach (User usuario in admins) {
            string body = MontarBodyCriacaoProjeto(usuario.Name, projeto);
            EnviarEmailCriacaoProjeto(usuario.EmailAddress, body);
        }
    }

    public void NotificarAtualizacaoFase(int projetoId)
    {
        Projeto projeto = _repository.GetAll()
                    .Include(x => x.Fase)
                    .Include(x => x.Concessionaria)
                    .Include(x => x.Cliente)
                    .Include(x => x.Tenant)
                    .FirstOrDefault(x => x.Id == projetoId);

        // NOTIFICA PARCEIRO QUE ESTA VINCULADO AO PROJETO E TODOS USUARIOS ADMIN
        List<User> users = _userRepository.GetAll()
            .Where( x => 
            x.Roles.Select(x => x.RoleId).Contains(1))
            .ToList();

        foreach (User usuario in users) {
            string body = MontarBody(usuario.Name, projeto);
            EnviarEmail(usuario.EmailAddress, body);
        }

        NotificarParceiro(projeto);
    }

    private void NotificarParceiro(Projeto projeto)
    {
        string body = MontarBody(projeto.Tenant.Fantasia, projeto);
        EnviarEmail(projeto.Tenant.Email, body);
    }

    private async Task EnviarEmail(string destinatario, string body)
    {
        await _emailSender.SendAsync(
                            to: destinatario,
                            subject: Properties.Resources.subjectAtualizacaoFase,
                            body: body,
                            from: "BBL Gestão",
                            isBodyHtml: true);
    }
    
    private async Task EnviarEmailCriacaoProjeto(string destinatario, string body)
    {
        await _emailSender.SendAsync(
            to: destinatario,
            subject: Properties.Resources.subjectCriacaoProjeto,
            body: body,
            from: "BBL Gestão",
            isBodyHtml: true);
    }

    private string MontarBody(string nome, Projeto projeto)
    {
        return string.Format(Properties.Resources.atualizacaoFaseProjeto,
                nome,
                projeto.Id,
                projeto.Cliente.Nome,
                projeto.Concessionaria.Nome,
                projeto.Valor.Value.ToString("C", CultureInfo.CurrentCulture),
                projeto.Fase.Nome
                );
    }
    
    private string MontarBodyCriacaoProjeto(string nome, Projeto projeto)
    {
        return string.Format(Properties.Resources.criacaoProjeto,
            nome,
            projeto.Tenant.Name
        );
    }
}