using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Enums;
using BBL.Models.Cobrancas;
using BBL.Projetos.Dto;

namespace BBL.Cobrancas.Dto;


[AutoMap(typeof(Cobranca))]
public class CobrancaDto : EntityDto<int>
{
    public int TenantId { get; set; }
    public string ParceiroNome { get; set; }
    public DateTime? DataGeracao { get; set; }
    public double ValorTotal { get; set; }
    public StatusPagamento? StatusPagamento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public IList<ProjetoDto> Projetos { get; set; }
    public IList<int> ProjetosId { get; set; }
    public int QuantidadeProjetos { get; set; }
}