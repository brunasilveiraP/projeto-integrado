
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Enderecos.Dto;
using BBL.Enums;
using BBL.Models.Parceiros;
using System.Collections.Generic;

namespace BBL.Parceiros.Dto;

[AutoMap(typeof(Parceiro))]
public class ParceiroDto : EntityDto<int>
{
    public string Cnpj { get; set; }
    public string RazaoSocial { get; set; }
    public string Fantasia { get; set; }
    public string Email { get; set; }
    public string Telefone1 { get; set; }
    public string Telefone2 { get; set; }
    public string InscricaoEstadual { get; set; }
    public string InscricaoMunicipal { get; set; }
    public int? EnderecoId { get; set; }
    public EnderecoDto Endereco { get; set; }
    public string CidadeUf { get; set; }
    public Status Status { get; set; }
    public string  Name { get; set; }
    public string TenancyName { get; set; }
    public SimNao PagaTrt { get; set; }
    public IList<int> DiasPagamento { get; set; }
    public IList<int> TabelasPreco { get; set; }
}