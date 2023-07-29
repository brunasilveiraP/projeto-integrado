using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using BBL.Models.Fases;
using BBL.Models.Projetos;

namespace BBL.Models.Concessionarias.Service;

public class ConcessionariaService : DomainService, IConcessionariaService
{
    private readonly IRepository<Fase, int> _repository;
    private readonly IRepository<Projeto, int> _projetoRepository;

    public ConcessionariaService(
        IRepository<Fase, int> repository,
        IRepository<Projeto, int> projetoRepository
        )
    {
        _repository = repository;
        _projetoRepository = projetoRepository;
    }

    public void CheckDelete(int id)
    {
        var existVinculoProjeto = _projetoRepository.GetAll().Any(x => x.ConcessionariaId == id);
        if (existVinculoProjeto)
            throw new UserFriendlyException("Exclusão não permitida! Concessionária já vinculada a um projeto.");
    }
}