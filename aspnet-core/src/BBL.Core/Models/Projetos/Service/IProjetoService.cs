using Abp.Domain.Services;

namespace BBL.Models.Projetos.Service;

public interface IProjetoService : IDomainService
{
    void CalcularValorProjeto(int projetoId);

    void NotificarAtualizacaoFase(int projetoId);
    
    void NotificarCriacaoProjeto(int projetoId);

}