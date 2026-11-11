using Pedidos.Api.Data;
using Pedidos.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pedidos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly PedidosDbContext _context;
        public PedidosController(PedidosDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> Obtener()
            => Ok(await _context.Pedidos.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Pedido>> ObtenerPorId(int id)
        {
            var item = await _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(x => x.IdPedido == id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> Crear(Pedido item)
        {
            if (!await _context.Clientes.AnyAsync(x => x.IdCliente == item.IdCliente))
                return BadRequest("El cliente no existe.");
            
            _context.Pedidos.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = item.IdPedido }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, Pedido item)
        {
            if (id != item.IdPedido) return BadRequest("El ID de la ruta no coincide.");
            var actual = await _context.Pedidos.FindAsync(id);
            if (actual is null) return NotFound();
            if (!await _context.Clientes.AnyAsync(x => x.IdCliente == item.IdCliente))
                return BadRequest("El cliente no existe.");
            actual.IdCliente = item.IdCliente;
            actual.Fecha = item.Fecha;
            actual.Total = item.Total;
            actual.Estado = item.Estado;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var item = await _context.Pedidos.FindAsync(id);
            if (item is null) return NotFound();
            if (await _context.DetallesPedido.AnyAsync(x => x.IdPedido == id))
                return Conflict("El pedido tiene detalles asociados.");
            _context.Pedidos.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
