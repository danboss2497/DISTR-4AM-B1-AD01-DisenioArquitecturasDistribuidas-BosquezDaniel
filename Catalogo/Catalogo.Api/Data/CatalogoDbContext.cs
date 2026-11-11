using Catalogo.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Data
{
    public class CatalogoDbContext : DbContext
    {
        public CatalogoDbContext(
            DbContextOptions<CatalogoDbContext> options) : base(options)
        {
        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>()
                .HasOne(producto => producto.Categoria)
                .WithMany()
                .HasForeignKey(producto => producto.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Producto>()
                .HasOne(producto => producto.Marca)
                .WithMany()
                .HasForeignKey(producto => producto.IdMarca)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}