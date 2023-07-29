using System;
using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BBL.Enums;
using BBL.Models.Anexos;
using BBL.Models.Clientes;
using BBL.Models.Cobrancas;
using BBL.Models.Concessionarias;
using BBL.Models.Fases;
using BBL.Models.Parceiros;

namespace BBL.Models.Projetos;

public class Projeto : AuditedEntity<int>, IMustHaveTenant
{
    public int? FaseId { get; set; }
    public Fase Fase { get; set; }
    public int? ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public int? CobrancaId { get; set; }
    public Cobranca Cobranca { get; set; }
    public int? ConcessionariaId { get; set; }
    public Concessionaria Concessionaria { get; set; }
    public IList<Anexo> Anexos { get; set; }
    public string Protocolo { get; set; }
    public string FabricanteInversor { get; set; }
    public string ModeloInversor { get; set; }
    public double PotenciaInversor { get; set; }
    public string FabricanteModulo { get; set; }
    public string ModeloModulo { get; set; }
    public int QuantidadeModulo { get; set; }
    public double PotenciaModulo { get; set; }
    public TipoGeracao TipoGeracao { get; set; }
    public TipoAterramento TipoAterramento { get; set; }
    public string NumeroPosteUc { get; set; }
    public string NumeroTransformador { get; set; }
    public string NumeroUc1 { get; set; }
    public double PercentualUc1 { get; set; }
    public string NumeroUc2 { get; set; }
    public double PercentualUc2 { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public double? Valor { get; set; }
    public StatusPagamento? StatusPagamento { get; set; }
    public double? ValorTrt { get; set; }
    public StatusPagamento? StatusPagamentoTrt { get; set; }
    public Parceiro Tenant { get; set; }
    public int TenantId { get; set; }
    public DateTime? DataCriacao { get; set; }
    public DateTime? DataHomologacao { get; set; }
    public List<Observacao> Observacoes { get; set; }
}