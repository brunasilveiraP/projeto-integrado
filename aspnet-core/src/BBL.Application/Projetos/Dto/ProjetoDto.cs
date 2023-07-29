using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Anexos.Dto;
using BBL.Clientes.Dto;
using BBL.Enums;
using BBL.Models.Projetos;

namespace BBL.Projetos.Dto;

[AutoMap(typeof(Projeto))]
public class ProjetoDto : EntityDto<int>
{
    public int FaseId { get; set; }
    public string FaseNome { get; set; }
    public int? ClienteId { get; set; }
    public CreateClienteDto Cliente { get; set; }
    public int ConcessionariaId { get; set; }
    public string ConcessionariaNome { get; set; }
    public int TenantId { get; set; }
    public string ParceiroNome { get; set; }
    public List<AnexoDto> Anexos { get; set; }
    public string Protocolo { get; set; }
    public string FabricanteInversor { get; set; }
    public string ModeloInversor { get; set; }
    public double? PotenciaInversor { get; set; }
    public string FabricanteModulo { get; set; }
    public string ModeloModulo { get; set; }
    public int QuantidadeModulo { get; set; }
    public double? PotenciaModulo { get; set; }
    public TipoGeracao TipoGeracao { get; set; }
    public TipoAterramento TipoAterramento { get; set; }
    public string NumeroPosteUc { get; set; }
    public string NumeroTransformador { get; set; }
    public string NumeroUc1 { get; set; }
    public double? PercentualUc1 { get; set; }
    public string NumeroUc2 { get; set; }
    public double? PercentualUc2 { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public double? Valor { get; set; }
    public StatusPagamento? StatusPagamento { get; set; }
    public double? ValorTrt { get; set; }
    public StatusPagamento? StatusPagamentoTrt { get; set; }
    public double areaTotal { get; set; }
    public DateTime? DataCriacao { get; set; }
    public DateTime? DataHomologacao { get; set; }
}