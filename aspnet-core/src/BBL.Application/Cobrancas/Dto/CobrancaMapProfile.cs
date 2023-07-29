using AutoMapper;
using BBL.Enums;
using BBL.Models.Cobrancas;
using System;
using System.Linq;

namespace BBL.Cobrancas.Dto;

public class CobrancaMapProfile : Profile 

{
    
    public CobrancaMapProfile()
    {

        CreateMap<CobrancaDto, Cobranca>()
            .ForMember(x => x.DataGeracao,
                opt => opt.MapFrom(x => x.DataGeracao == null ? DateTime.Now : x.DataGeracao))
            ;

        CreateMap<Cobranca, CobrancaDto>()
            .ForMember(x => x.ParceiroNome,
                opt => opt.MapFrom(x => x.Tenant.Fantasia))
            .ForMember(x => x.ProjetosId,
                opt => opt.MapFrom(x => x.Projetos.Select(x => x.Id)))
             .ForMember(x => x.QuantidadeProjetos,
                opt => opt.MapFrom(x => x.Projetos.Count()))
             .ForMember(x => x.Projetos,
                opt => opt.Ignore())
            ;

    }
    
}