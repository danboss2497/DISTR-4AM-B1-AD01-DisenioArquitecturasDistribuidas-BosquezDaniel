using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Pedidos.Api.Models
{
    [Index(nameof(Cedula), IsUnique = true)]
    public class Cliente
    {
        [Key] public int IdCliente { get; set; }
        [Required, StringLength(150)] public string Nombres { get; set; } = string.Empty;
        [Required, StringLength(20)] public string Cedula { get; set; } = string.Empty;
        [Required, EmailAddress, StringLength(150)] public string Correo { get; set; } = string.Empty;
    }
}
