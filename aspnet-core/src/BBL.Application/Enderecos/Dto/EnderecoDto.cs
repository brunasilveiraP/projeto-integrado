using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BBL.Models.Enderecos;
using JetBrains.Annotations;

namespace BBL.Enderecos.Dto;


[AutoMap(typeof(Endereco))]
public class EnderecoDto : EntityDto<int>
{
    public string Cep { get; set; }
    public string Uf { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string Complemento { get; set; }
    
}