using Abp.Application.Services.Dto;
using AutoMapper;
using BBL.Enums;
using BBL.Models.Projetos;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BBL.Projetos.Dto;

[AutoMap(typeof(Projeto))]
public class ProjetoFilterDto
{
    [CanBeNull] public List<int> Parceiros { get; set; }
    public List<int> Fases { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}