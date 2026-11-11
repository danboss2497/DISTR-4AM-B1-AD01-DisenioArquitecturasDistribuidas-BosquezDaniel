using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Models
{
    [Index(nameof(IdProducto), nameof(IdBodega), IsUnique = true)]
    public class Existencia
    {
        [Key] public int IdExistencia { get; set; }
        [Range(1, int.MaxValue)] public int IdProducto { get; set; }
        [Range(1, int.MaxValue)] public int IdBodega { get; set; }
        [Range(0, int.MaxValue)] public int Stock { get; set; }
        public Bodega? Bodega { get; set; }
    }
}
