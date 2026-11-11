using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos.Api.Models
{
    public class Pedido
    {
        [Key] public int IdPedido { get; set; }
        [Range(1, int.MaxValue)] public int IdCliente { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(12,2)")] public decimal Total { get; set; }
        [Required, StringLength(30)] public string Estado { get; set; } = "Pendiente";
        public Cliente? Cliente { get; set; }
    }
}
