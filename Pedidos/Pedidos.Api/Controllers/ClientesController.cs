using Pedidos.Api.Data;
using Pedidos.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pedidos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly PedidosDbContext _context;
        public ClientesController(PedidosDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Obtener()
            => Ok(await _context.Clientes.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cliente>> ObtenerPorId(int id)
        {
            var item = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.IdCliente == id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Crear(Cliente item)
        {
            if (await _context.Clientes.AnyAsync(x => x.Cedula == item.Cedula))
                return Conflict("La cédula ya está registrada.");
            _context.Clientes.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = item.IdCliente }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, Cliente item)
        {
            if (id != item.IdCliente) return BadRequest("El ID de la ruta no coincide.");
            var actual = await _context.Clientes.FindAsync(id);
            if (actual is null) return NotFound();
            if (await _context.Clientes.AnyAsync(x => x.Cedula == item.Cedula && x.IdCliente != id))
                return Conflict("La cédula ya está registrada.");
            actual.Nombres = item.Nombres;
            actual.Cedula = item.Cedula;
            actual.Correo = item.Correo;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var item = await _context.Clientes.FindAsync(id);
            if (item is null) return NotFound();
            if (await _context.Pedidos.AnyAsync(x => x.IdCliente == id))
                return Conflict("El cliente tiene pedidos asociados.");
            _context.Clientes.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
