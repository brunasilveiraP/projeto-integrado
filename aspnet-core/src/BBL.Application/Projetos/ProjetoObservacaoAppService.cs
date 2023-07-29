using AApecan.ContribuinteComentariosMensageiro.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using BBL.Models.Projetos;
using BBL.Projetos.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BBL.Projetos;

public class ProjetoObservacaoAppService : AsyncCrudAppService<Observacao, ProjetoObservacaoDto, int, GetProjetoObservacaoDto>
{
    public ProjetoObservacaoAppService(IRepository<Observacao, int> repository) : base(repository)
    {
    }

    protected override IQueryable<Observacao> CreateFilteredQuery(GetProjetoObservacaoDto input)
    {
        return base.CreateFilteredQuery(input)
            .WhereIf(input.ProjetoId > 0, x => x.ProjetoId.Equals(input.ProjetoId)).OrderBy(x => x.Fixed).OrderByDescending(x => x.Id);
    }


    public override async  Task<ProjetoObservacaoDto> CreateAsync(ProjetoObservacaoDto input)
    {
        try
        {
            var t = await base.CreateAsync(input);
            CurrentUnitOfWork.SaveChanges();
            return t;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}