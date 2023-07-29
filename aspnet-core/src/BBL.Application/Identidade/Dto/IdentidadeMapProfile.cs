using AutoMapper;

namespace BBL.Identidade.Dto
{
    public class IdentidadeMapProfile : Profile
    {
        public IdentidadeMapProfile()
        {
            CreateMap<AcessarSistemaRetornoDto, AcessarSistemaRetornoDto>();
        }
    }
}
