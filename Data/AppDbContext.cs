using Microsoft.EntityFrameworkCore;

namespace CommerceApi.Data
{
    public class AppDbContext :  DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<User> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>()
            .HasIndex(x => x.Id)
            .IsUnique();
            modelBuilder.Entity<User>()
            .HasIndex(x => x.Id)
            .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

            modelBuilder.Entity<Produto>()
            .HasData(
                new Produto { Id = 1, Nome = "Caderno", Preco = 10.5M, Estoque = 10, Ativo = true },
                new Produto { Id = 2, Nome = "Mochila", Preco = 40M, Estoque = 20, Ativo = true },
                new Produto { Id = 3, Nome = "Borracha", Preco = 7.5M, Estoque = 15, Ativo = true });
        }
    }
}