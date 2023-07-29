using Abp.Domain.Services;

namespace BBL.Models.TabelaPrecos.Service;

public interface ITabelaPrecoService : IDomainService
{
    void CheckDelete(int tabelaPrecoId);
}