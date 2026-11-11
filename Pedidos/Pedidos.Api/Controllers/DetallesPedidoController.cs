using Pedidos.Api.Data;
using Pedidos.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pedidos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetallesPedidoController : ControllerBase
    {
        private readonly PedidosDbContext _context;
        public DetallesPedidoController(PedidosDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> Obtener()
            => Ok(await _context.DetallesPedido.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetallePedido>> ObtenerPorId(int id)
        {
            var item = await _context.DetallesPedido.AsNoTracking().FirstOrDefaultAsync(x => x.IdDetalle == id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<DetallePedido>> Crear(DetallePedido item)
        {
            if (!await _context.Pedidos.AnyAsync(x => x.IdPedido == item.IdPedido))
                return BadRequest("El pedido no existe.");
            
            _context.DetallesPedido.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = item.IdDetalle }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, DetallePedido item)
        {
            if (id != item.IdDetalle) return BadRequest("El ID de la ruta no coincide.");
            var actual = await _context.DetallesPedido.FindAsync(id);
            if (actual is null) return NotFound();
            if (!await _context.Pedidos.AnyAsync(x => x.IdPedido == item.IdPedido))
                return BadRequest("El pedido no existe.");
            actual.IdPedido = item.IdPedido;
            actual.IdProducto = item.IdProducto;
            actual.Cantidad = item.Cantidad;
            actual.PrecioUnitario = item.PrecioUnitario;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var item = await _context.DetallesPedido.FindAsync(id);
            if (item is null) return NotFound();
            
            _context.DetallesPedido.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
