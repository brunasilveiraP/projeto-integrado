using AutoMapper;
using BBL.Enums;
using BBL.Models.Projetos;
using BBL.Models.TabelaPrecos;
using BBL.Projetos.Dto;
using BBL.TabelaPrecos.Dto;
using System;
using System.Globalization;

namespace BBL.Projetos;

public class ProjetoMapProfile : Profile
{
    public ProjetoMapProfile()
    {
        CreateMap<Projeto, SearchProjetoDto>()
            .ForMember(x => x.ClienteNome,
                opt => opt.MapFrom(x => x.Cliente.Nome))
            .ForMember(x => x.ConcessionariaNome,
                opt => opt.MapFrom(x => x.Concessionaria.Nome))
            .ForMember(x => x.FaseNome,
                opt => opt.MapFrom(x => x.Fase.Nome))
            .ForMember(x => x.ParceiroNome,
                opt => opt.MapFrom(x => x.Tenant.Fantasia))
            ;

        CreateMap<CreateProjetoDto, Projeto>()
            .ForMember(x => x.DataCriacao, opt => opt.MapFrom(x => x.DataCriacao == null ? DateTime.Now : x.DataCriacao))
            .ForMember(x => x.StatusPagamento, opt => opt.MapFrom(x => x.StatusPagamento == null ? StatusPagamento.NaoFaturado : x.StatusPagamento));             
        
        CreateMap<ProjetoDto, Projeto>()
            .ForMember(x => x.Anexos, opt => opt.Ignore());

        CreateMap<Projeto, ProjetoDto>()
            .ForMember(x => x.ConcessionariaNome, opt => opt.MapFrom(x => x.Concessionaria.Nome))
            .ForMember(x => x.FaseNome, opt => opt.MapFrom(x => x.Fase.Nome))
            .ForMember(x => x.ParceiroNome, opt => opt.MapFrom(x => x.Tenant.Fantasia))
            .ForMember(x => x.Cliente, opt => opt.MapFrom(x => x.Cliente))
            .ForMember(x => x.DataCriacao, opt => opt.MapFrom(x => x.DataCriacao == null ? DateTime.Now : x.DataCriacao));

        CreateMap<Projeto, ProjetoContempladoDto>()
            .ForMember(x => x.Descricao,
                opt => opt.MapFrom(
                    x =>
                        $"{x.Cliente.Nome} - {x.Concessionaria.Nome} - {x.Valor.Value.ToString("C", CultureInfo.CurrentCulture)} - {x.PotenciaModulo} (KWP) - {x.Fase.Nome}"))
            ;

    }
}