using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.Concessionarias;

namespace BBL.Concessionarias.Dto;


[AutoMap(typeof(Concessionaria))]
public class ConcessionariaDto : EntityDto
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }

}