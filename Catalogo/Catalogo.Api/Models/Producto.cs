using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Models
{
    [Index(nameof(SKU), IsUnique = true)]
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría válida.")]
        public int IdCategoria { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una marca válida.")]
        public int IdMarca { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "El SKU es obligatorio.")]
        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10,2)")]
        [Range(typeof(decimal), "0.01", "99999999.99",
            ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
        public Categoria? Categoria { get; set; }
        public Marca? Marca { get; set; }
    }
}