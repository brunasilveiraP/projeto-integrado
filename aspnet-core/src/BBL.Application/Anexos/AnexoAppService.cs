using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BBL.Anexos.Dto;
using BBL.Models.Anexos;
using Microsoft.EntityFrameworkCore;

namespace BBL.Anexos;

public class AnexoAppService : AsyncCrudAppService<Anexo, AnexoDto, int>
{
    public AnexoAppService(IRepository<Anexo, int> repository) : base(repository)
    {
    }

    protected override IQueryable<Anexo> CreateFilteredQuery(PagedAndSortedResultRequestDto input)
    {
        return base.CreateFilteredQuery(input).Include(x =>x.TipoAnexo);
    }

    public IEnumerable<AnexoDto> GetAllByProjeto(int projetoId)
    {
        var anexos = Repository.GetAll().Include(x => x.TipoAnexo).Where(x => x.ProjetoId == projetoId).ToList();
        return ObjectMapper.Map<IEnumerable<AnexoDto>>(anexos);
    }

    public override Task<AnexoDto> CreateAsync(AnexoDto input)
    {
        try
        {
            var anexo =base.CreateAsync(input);
            return anexo;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}