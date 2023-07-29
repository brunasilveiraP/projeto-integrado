using Abp.Domain.Services;

namespace BBL.Models.TipoAnexos.Service;

public interface ITipoAnexoService : IDomainService
{
    void CheckDelete(int id);
}