using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using BBL.Authorization.Roles;
using BBL.Authorization.Users;
using BBL.Models.Anexos;
using BBL.Models.Clientes;
using BBL.Models.Cobrancas;
using BBL.Models.Concessionarias;
using BBL.Models.Enderecos;
using BBL.Models.Fases;
using BBL.Models.Parceiros;
using BBL.Models.Projetos;
using BBL.Models.TabelaPrecos;
using BBL.Models.TipoAnexos;
using BBL.MultiTenancy;

namespace BBL.EntityFrameworkCore
{
    public class BblDbContext : AbpZeroDbContext<Parceiro, Role, User, BblDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public DbSet<Anexo> Anexos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cobranca> Cobrancas { get; set; }
        public DbSet<Concessionaria> Concessionarias { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Fase> Fases { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<TabelaPreco> TabelaPrecos { get; set; }
        public DbSet<TipoAnexo> TipoAnexos { get; set; }
        public DbSet<ParceiroTabelaPreco> ParceiroTabelaPrecos { get; set; }
        public DbSet<DiaPagamento> DiasPagamento { get; set; }
        public DbSet<Observacao> Observacao { get; set; }

        public BblDbContext(DbContextOptions<BblDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
                
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>()
                .HasOne(x => x.Endereco)
                .WithOne(x=> x.Cliente)
                .HasForeignKey<Cliente>(x=> x.EnderecoId);

            modelBuilder.Entity<Endereco>()
                .HasOne(x => x.Cliente)
                .WithOne(x => x.Endereco)
                ;
            
            modelBuilder.Entity<Parceiro>()
                .HasOne(x => x.Endereco)
                .WithOne(x=> x.Parceiro)
                .HasForeignKey<Parceiro>(x=> x.EnderecoId);

            modelBuilder.Entity<Parceiro>()
                .HasMany(x => x.ParceiroTabelasPreco)
                .WithOne(x => x.Parceiro);
            
            modelBuilder.Entity<Parceiro>()
                .HasMany(x => x.DiasPagamento)
                .WithOne(x => x.Parceiro).OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<TabelaPreco>()
                .HasMany(x => x.ParceiroTabelasPreco)
                .WithOne(x => x.TabelaPreco);
            
            modelBuilder.Entity<Parceiro>()
                .HasMany(x => x.Cobrancas)
                .WithOne(x => x.Tenant);
            

            modelBuilder.Entity<Endereco>()
                .HasOne(x => x.Parceiro)
                .WithOne(x => x.Endereco)
                ;

            modelBuilder.Entity<Cobranca>()
                .HasMany(x => x.Projetos)
                .WithOne(x => x.Cobranca)
                .OnDelete(DeleteBehavior.NoAction)
                ;
            
            modelBuilder.Entity<Cobranca>()
                .HasOne(x => x.Tenant)
                .WithMany(x => x.Cobrancas)
                ;

        }
    }
}
