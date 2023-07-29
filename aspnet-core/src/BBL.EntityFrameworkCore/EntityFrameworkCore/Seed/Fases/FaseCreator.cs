using System.Collections.Generic;
using System.Linq;
using BBL.Models.Concessionarias;
using BBL.Models.Fases;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace BBL.EntityFrameworkCore.Seed.Fases
{
    public class FaseCreator
    {
        private readonly BblDbContext _context;

        public FaseCreator(BblDbContext context)
        {
            _context = context;
        }

        public async void Create()
        {
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                var registros = _context.Fases.Count();

                if (registros == 0)
                {
                    _context.Fases.AddRange(new List<Fase>
                    {
                        CreateConcessionaria("Documento em Análise Técnica"),
                        CreateConcessionaria("Documento Incompleto"),
                        CreateConcessionaria("Documento Assinatura Inválida"),
                        CreateConcessionaria("Documento Aguardando Assinatura"),
                        CreateConcessionaria("Documento Envio Concessionária"),
                        CreateConcessionaria("Documento Reprovado"),
                        CreateConcessionaria("Documento Aguardando Orçamento"),
                        CreateConcessionaria("Solicitação da Inspeção"),
                        CreateConcessionaria("Inspeção Reprovada"),
                        CreateConcessionaria("Inspeção Aprovada"),
                        CreateConcessionaria("Parecer de acesso Disponível"),
                        CreateConcessionaria("Aguardando Homologação"),
                        CreateConcessionaria("Projeto Homologado"),
                    });
                   _context.SaveChanges();
                }

            }
        }
        private Fase CreateConcessionaria(string nome) => new Fase { Nome = nome };
    }
}


