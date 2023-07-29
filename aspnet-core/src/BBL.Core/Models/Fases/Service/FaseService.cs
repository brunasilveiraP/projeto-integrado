using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using BBL.Models.Projetos;

namespace BBL.Models.Fases.Service;

public class FaseService : DomainService, IFaseService
{
    private readonly IRepository<Fase, int> _repository;
    private readonly IRepository<Projeto, int> _projetoRepository;

    public FaseService(
        IRepository<Fase, int> repository,
        IRepository<Projeto, int> projetoRepository
        )
    {
        _repository = repository;
        _projetoRepository = projetoRepository;
    }

    public void CheckDelete(int id)
    {
        var existVinculoProjeto = _projetoRepository.GetAll().Any(x => x.FaseId == id);
        if (existVinculoProjeto)
            throw new UserFriendlyException("Exclusão não permitida! Fase já vinculada a um projeto.");
    }
}