using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using BBL.Models.Parceiros;
using BBL.Models.Projetos;

namespace BBL.Models.TipoAnexos.Service;

public class TipoAnexoService : DomainService, ITipoAnexoService
{
    private readonly IRepository<TipoAnexo, int> _repository;
    private readonly IRepository<Parceiro, int> _parceiroRepository;
    private readonly IRepository<Projeto, int> _projetoRepository;

    public TipoAnexoService(
        IRepository<TipoAnexo, int> repository,
        IRepository<Parceiro, int> parceiroRepository,
        IRepository<Projeto, int> projetoRepository
        )
    {
        _repository = repository;
        _parceiroRepository = parceiroRepository;
        _projetoRepository = projetoRepository;
    }

    public void CheckDelete(int id)
    {
        var existVinculoProjeto = _projetoRepository.GetAll().Any(x => x.Anexos.Any(x => x.Id == id));
        if (existVinculoProjeto)
            throw new UserFriendlyException("Exclusão não permitida! Tipo de Anexo já vinculado a um projeto.");
    }
}