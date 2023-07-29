using System.Collections.Generic;
using Abp.MultiTenancy;
using BBL.Authorization.Users;
using BBL.Enums;
using BBL.Models.Cobrancas;
using BBL.Models.Enderecos;
using BBL.Models.Projetos;
using JetBrains.Annotations;

namespace BBL.Models.Parceiros;

public class Parceiro : AbpTenant<User>
{
    public string Cnpj { get; set; }
    public string RazaoSocial { get; set; }
    public string Fantasia { get; set; }
    public string Email { get; set; }
    public string Telefone1 { get; set; }
    [CanBeNull] public string Telefone2 { get; set; }
    [CanBeNull] public string InscricaoEstadual { get; set; }
    [CanBeNull] public string InscricaoMunicipal { get; set; }
    public int? EnderecoId { get; set; }
    public Endereco Endereco { get; set; }
    public SimNao PagaTrt { get; set; }
    public IList<DiaPagamento> DiasPagamento { get; set; }
    public Status Status { get; set; }
    public IList<ParceiroTabelaPreco> ParceiroTabelasPreco { get; set; }
    public IList<Cobranca> Cobrancas { get; set; }
    public IList<Projeto> Projetos { get; set; }
}