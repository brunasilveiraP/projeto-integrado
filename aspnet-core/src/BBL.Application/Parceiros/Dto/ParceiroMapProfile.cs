using System;
using System.Linq;
using AutoMapper;
using BBL.Enderecos.Dto;
using BBL.Models.Parceiros;

namespace BBL.Parceiros.Dto;

public class ParceiroMapProfile : Profile 

{
    
    public ParceiroMapProfile()
    {

        CreateMap<ParceiroDto, Parceiro>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Fantasia))
            .ForMember(x => x.TenancyName, opt => opt.MapFrom(x => x.Fantasia))
            .ForMember(x => x.ParceiroTabelasPreco, opt => opt.Ignore())
            
            ;
        
        CreateMap<ParceiroCreateDto, Parceiro>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Fantasia))
            .ForMember(x => x.TenancyName, opt => opt.MapFrom(x => x.Fantasia))
            .ForMember(x => x.ParceiroTabelasPreco, opt => opt.MapFrom(x => x.TabelasPreco.Select(y => new ParceiroTabelaPreco(x.Id, y))))
            .ForMember(x => x.DiasPagamento,
                opt => opt.MapFrom(x => x.DiasPagamento.Select(y => new DiaPagamento(y))))
            ;
        
        CreateMap<Parceiro, ParceiroDto>()
            .ForMember(x => x.CidadeUf, 
                opt => opt.MapFrom(x => x.Endereco.Cidade + " - " + x.Endereco.Uf))
            .ForMember(x => x.TabelasPreco,
                opt => opt.MapFrom(x => x.ParceiroTabelasPreco.Select(y => y.TabelaPrecoId)))
            .ForMember(x => x.DiasPagamento,
                opt => opt.MapFrom(x => x.DiasPagamento.Select(y => y.Dia)))
            ;
    }
    
}