using System.Collections.Generic;
using System.Linq;

namespace BBL.EntityFrameworkCore.Seed.TipoAnexo;

public class TipoAnexoCreator
{
    private readonly BblDbContext _context;

    public TipoAnexoCreator(BblDbContext context)
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
                _context.TipoAnexos.AddRange(new List<Models.TipoAnexos.TipoAnexo>
                {
                    CreateTipoAnexo("CNH - Frente"),
                    CreateTipoAnexo("CNH - Verso"),
                    CreateTipoAnexo("CNH"),
                    CreateTipoAnexo("Comprovante de Endereço"),
                    CreateTipoAnexo("RG"),
                    CreateTipoAnexo("Foto - Poste"),
                    CreateTipoAnexo("Localização"),
                    CreateTipoAnexo("Anexo E"),
                    CreateTipoAnexo("Anexo F"),

                });
                _context.SaveChanges();
            }

        }
    }
    private Models.TipoAnexos.TipoAnexo CreateTipoAnexo(string nome) => new Models.TipoAnexos.TipoAnexo() {  Nome= nome };
}