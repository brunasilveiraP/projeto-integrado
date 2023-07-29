using Abp.Application.Services.Dto;
using AutoMapper;
using BBL.Enums;
using BBL.Models.Projetos;

namespace BBL.Projetos.Dto;

[AutoMap(typeof(Projeto))]
public class SearchProjetoDto : EntityDto<int>
{
    public string ParceiroNome { get; set; }
    public string FaseNome { get; set; }
    public string ClienteNome { get; set; }
    public string ConcessionariaNome { get; set; }
    public StatusPagamento StatusPagamento { get; set; }
    public double Valor { get; set; }
}