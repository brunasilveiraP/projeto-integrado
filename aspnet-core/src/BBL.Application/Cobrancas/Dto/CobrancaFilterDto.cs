using AutoMapper;
using System;
using System.Collections.Generic;
using BBL.Enums;
using BBL.Models.Cobrancas;

namespace BBL.Projetos.Dto;

[AutoMap(typeof(Cobranca))]
public class CobrancaFilterDto
{
    public List<int> Parceiros { get; set; }
    public StatusPagamento? Status { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public DateTime? DataPagamentoInicio { get; set; }
    public DateTime? DataPagamentoFim { get; set; }
}