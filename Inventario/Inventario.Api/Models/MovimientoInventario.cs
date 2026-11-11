using System.ComponentModel.DataAnnotations;

namespace Inventario.Api.Models
{
    public class MovimientoInventario
    {
        [Key] public int IdMovimiento { get; set; }
        [Range(1, int.MaxValue)] public int IdProducto { get; set; }
        [Range(1, int.MaxValue)] public int IdBodega { get; set; }
        [Required, RegularExpression("^(Entrada|Salida)$")] public string Tipo { get; set; } = string.Empty;
        [Range(1, int.MaxValue)] public int Cantidad { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public Bodega? Bodega { get; set; }
    }
}
