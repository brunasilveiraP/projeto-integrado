using System.Globalization;
using AutoMapper;
using BBL.Models.TabelaPrecos;

namespace BBL.TabelaPrecos.Dto;

public class TabelaPrecoMapProfile : Profile 
{
    public TabelaPrecoMapProfile()
    {
        CreateMap<TabelaPreco, TabelaPrecoSimpleDto>()
            .ForMember(x => x.Descricao,
                opt => opt.MapFrom(
                    x => 
                        $"{x.KwpInicial} a {x.KwpFinal} - { x.Valor.ToString("C", CultureInfo.CurrentCulture)}"))
            ;
        
        CreateMap<TabelaPreco, TabelaPrecoDto>()
            .ForMember(x => x.Nome,
                opt => opt.MapFrom(
                    x => 
                        $"{x.KwpInicial} a {x.KwpFinal}"))
            ;
    }
    
}