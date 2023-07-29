using Abp.Domain.Services;

namespace BBL.Models.Concessionarias.Service;

public interface IConcessionariaService : IDomainService
{
    void CheckDelete(int id);
}