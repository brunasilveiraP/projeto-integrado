using BBL.EntityFrameworkCore.Seed.Concessionarias;
using BBL.EntityFrameworkCore.Seed.Fases;
using BBL.EntityFrameworkCore.Seed.Perfil;
using BBL.EntityFrameworkCore.Seed.TabelasPreco;

namespace BBL.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly BblDbContext _context;

        public InitialHostDbBuilder(BblDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            //new PerfilCreator(_context).Create();
            new ConcessionariaCreator(_context).Create();
            new FaseCreator(_context).Create();
            new TabelaPrecoCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
