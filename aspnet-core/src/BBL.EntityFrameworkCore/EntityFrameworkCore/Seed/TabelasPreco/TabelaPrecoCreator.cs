using System.Collections.Generic;
using System.Linq;
using BBL.Models.TabelaPrecos;

namespace BBL.EntityFrameworkCore.Seed.TabelasPreco;

public class TabelaPrecoCreator
{
    private readonly BblDbContext _context;

    public TabelaPrecoCreator(BblDbContext context)
    {
        _context = context;
    }

    public async void Create()
    {
        if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        {
            var registros = _context.TabelaPrecos.Count();

            if (registros == 0)
            {
                _context.TabelaPrecos.AddRange(new List<TabelaPreco>
                {
                    CreateTabelaPreco(1,10, 120.00),
                    CreateTabelaPreco(11,20, 100.00),
                    CreateTabelaPreco(21,30,  80.00),
                    CreateTabelaPreco(31,40,  70.00),
                    CreateTabelaPreco(41,50, 60.00),
                });
                _context.SaveChanges();
            }

        }
    }
    private TabelaPreco CreateTabelaPreco(uint inicial, uint final, double valor) => new TabelaPreco { KwpInicial = inicial, KwpFinal = final, Valor = valor};
}