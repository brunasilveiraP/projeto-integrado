using Abp.Application.Services;
using BBL.TipoAnexos.Dto;

namespace BBL.TipoAnexos;

public interface ITipoAnexoAppService : IAsyncCrudAppService<TipoAnexoDto>
{
    
}