using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Models;
namespace Pedidos.Api.Data
{
    public class PedidosDbContext : DbContext
    {
        public PedidosDbContext(DbContextOptions<PedidosDbContext> options) : base(options) { }
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>().HasOne(x => x.Cliente).WithMany().HasForeignKey(x => x.IdCliente).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DetallePedido>().HasOne(x => x.Pedido).WithMany().HasForeignKey(x => x.IdPedido).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
