using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.Projetos;

namespace BBL.Projetos.Dto;

[AutoMap(typeof(Projeto))]
public class ProjetoContempladoDto : EntityDto<int>
{
    public string Descricao { get; set; }
    public double Valor { get; set; }
}