using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using BBL.Models.Parceiros;

namespace BBL.Models.TabelaPrecos.Service;

public class TabelaPrecoService : DomainService, ITabelaPrecoService
{
    private readonly IRepository<TabelaPreco, int> _repository;

    public TabelaPrecoService(
        IRepository<TabelaPreco, int> repository
        )
    {
        _repository = repository;
    }

    public void CheckDelete(int tabelaPrecoId)
    {
    }
}