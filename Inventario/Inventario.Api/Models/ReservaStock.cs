using System.ComponentModel.DataAnnotations;

namespace Inventario.Api.Models
{
    public class ReservaStock
    {
        [Key] public int IdReserva { get; set; }
        [Range(1, int.MaxValue)] public int IdProducto { get; set; }
        [Range(1, int.MaxValue)] public int IdBodega { get; set; }
        [Range(1, int.MaxValue)] public int Cantidad { get; set; }
        [Required, StringLength(30)] public string Estado { get; set; } = "Activa";
        public Bodega? Bodega { get; set; }
    }
}
