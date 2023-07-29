using Abp.Domain.Services;

namespace BBL.Models.Fases.Service;

public interface IFaseService : IDomainService
{
    void CheckDelete(int id);
}