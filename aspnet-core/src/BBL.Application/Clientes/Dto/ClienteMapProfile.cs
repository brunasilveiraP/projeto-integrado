using AutoMapper;
using BBL.Models.Clientes;
using BBL.Models.Projetos;
using BBL.Projetos.Dto;

namespace BBL.Clientes.Dto;

public class ClienteMapProfile : Profile
{
    public ClienteMapProfile()
    {
        CreateMap<Cliente, CreateClienteDto>()
            ;

        CreateMap<CreateClienteDto, Cliente>();
        
    }
}