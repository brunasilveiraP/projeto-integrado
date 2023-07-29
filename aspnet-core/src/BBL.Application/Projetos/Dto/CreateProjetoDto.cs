using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Enums;
using BBL.Models.Projetos;
using System;
using System.Runtime.InteropServices;

namespace BBL.Projetos.Dto;

[AutoMap(typeof(Projeto))]
public class CreateProjetoDto : EntityDto<int>
{
    public int? TenantId { get; set; }
    public int FaseId { get; set; }
    public StatusPagamento? StatusPagamento { get; set; }
    public DateTime? DataCriacao { get; set; }
}