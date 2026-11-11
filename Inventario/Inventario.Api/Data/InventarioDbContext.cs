using Inventario.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Inventario.Api.Data
{
    public class InventarioDbContext : DbContext
    {
        public InventarioDbContext(DbContextOptions<InventarioDbContext> options) : base(options) { }
        public DbSet<Bodega> Bodegas => Set<Bodega>();
        public DbSet<Existencia> Existencias => Set<Existencia>();
        public DbSet<MovimientoInventario> MovimientosInventario => Set<MovimientoInventario>();
        public DbSet<ReservaStock> ReservasStock => Set<ReservaStock>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Existencia>().HasOne(x => x.Bodega).WithMany().HasForeignKey(x => x.IdBodega).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MovimientoInventario>().HasOne(x => x.Bodega).WithMany().HasForeignKey(x => x.IdBodega).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ReservaStock>().HasOne(x => x.Bodega).WithMany().HasForeignKey(x => x.IdBodega).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
