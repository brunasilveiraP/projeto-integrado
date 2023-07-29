using System;
using System.Collections.Generic;
using System.Linq;
using BBL.Models.Concessionarias;
using BBL.Models.Enderecos;

namespace BBL.EntityFrameworkCore.Seed.Concessionarias
{
    public class ConcessionariaCreator
    {
        private readonly BblDbContext _context;

        public ConcessionariaCreator(BblDbContext context)
        {
            _context = context;
        }

        public async void Create()
        {
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                var registros = _context.Concessionarias.Count();

                if (registros == 0)
                {
                    _context.Concessionarias.AddRange(new List<Concessionaria>
                    {
                        CreateConcessionaria("CPFL"),
                        CreateConcessionaria("Coopercocal"),
                        CreateConcessionaria("CEEE"),
                        CreateConcessionaria("Certel"),
                        CreateConcessionaria("Coopersul"),
                        CreateConcessionaria("Eletrocar"),
                        CreateConcessionaria("Creral"),
                        CreateConcessionaria("Hidropan"),
                        
                    });
                    _context.SaveChanges();
                }

            }
        }

        private Concessionaria CreateConcessionaria(string nome) =>
            new Concessionaria
            {
                Nome= nome,
            };
    }
}


