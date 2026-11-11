using System.ComponentModel.DataAnnotations;

namespace Catalogo.Api.Models
{
    public class Marca
    {
        [Key]
        public int IdMarca { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? PaisOrigen { get; set; }
    }
}
