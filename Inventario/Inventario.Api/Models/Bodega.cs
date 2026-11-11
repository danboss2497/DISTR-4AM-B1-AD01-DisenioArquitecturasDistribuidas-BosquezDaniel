using System.ComponentModel.DataAnnotations;

namespace Inventario.Api.Models
{
    public class Bodega
    {
        [Key] public int IdBodega { get; set; }
        [Required, StringLength(120)] public string Nombre { get; set; } = string.Empty;
        [Required, StringLength(100)] public string Ciudad { get; set; } = string.Empty;
        [Required, StringLength(200)] public string Direccion { get; set; } = string.Empty;
    }
}
