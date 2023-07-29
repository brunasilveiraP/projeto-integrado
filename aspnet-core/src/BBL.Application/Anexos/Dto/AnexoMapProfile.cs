using AutoMapper;
using BBL.Models.Anexos;

namespace BBL.Anexos.Dto;

public class AnexoMapProfile : Profile
{
    public 
        AnexoMapProfile()
    {

        CreateMap<Anexo, AnexoDto>()
            .ForMember(x => x.Arquivo,
                opt => opt.MapFrom(x => x.Arquivo))
            .ForMember(x => x.NomeArquivo,
                opt => opt.MapFrom(x => x.NomeArquivo))
            .ReverseMap()
            ;
    }
}