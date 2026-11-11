using System.ComponentModel.DataAnnotations;

namespace Catalogo.Api.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }       
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
