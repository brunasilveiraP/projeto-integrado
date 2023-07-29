using AutoMapper;
using BBL.Models.Enderecos;

namespace BBL.Enderecos.Dto;

public class EnderecoMapProfile : Profile
{
    public EnderecoMapProfile()
    {
        CreateMap<Endereco, EnderecoDto>()
            .ForMember(x=> x.Bairro,
                opt=>opt.MapFrom(x=> x.Bairro))
            .ForMember(x=> x.Cep,
                opt=> opt.MapFrom(x=> x.Cep))
            .ForMember(x=> x.Cidade,
                opt=> opt.MapFrom(x=> x.Cidade))
            .ForMember(x=> x.Complemento,
                opt=> opt.MapFrom(x=> x.Complemento))
            .ForMember(x=> x.Numero,
                opt=> opt.MapFrom(x=> x.Numero))
            .ForMember(x=> x.Rua,
                opt=> opt.MapFrom(x=> x.Rua))
            .ForMember(x=> x.Uf,
                opt=> opt.MapFrom(x=> x.Uf))
            ;
    }
}