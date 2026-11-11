using Inventario.Api.Data;
using Inventario.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasStockController : ControllerBase
    {
        private readonly InventarioDbContext _context;
        public ReservasStockController(InventarioDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaStock>>> Obtener()
            => Ok(await _context.ReservasStock.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReservaStock>> ObtenerPorId(int id)
        {
            var item = await _context.ReservasStock.AsNoTracking().FirstOrDefaultAsync(x => x.IdReserva == id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ReservaStock>> Crear(ReservaStock item)
        {
            if (!await _context.Bodegas.AnyAsync(x => x.IdBodega == item.IdBodega))
                return BadRequest("La bodega no existe.");
            
            _context.ReservasStock.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = item.IdReserva }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, ReservaStock item)
        {
            if (id != item.IdReserva) return BadRequest("El ID de la ruta no coincide.");
            var actual = await _context.ReservasStock.FindAsync(id);
            if (actual is null) return NotFound();
            if (!await _context.Bodegas.AnyAsync(x => x.IdBodega == item.IdBodega))
                return BadRequest("La bodega no existe.");
            actual.IdProducto = item.IdProducto;
            actual.IdBodega = item.IdBodega;
            actual.Cantidad = item.Cantidad;
            actual.Estado = item.Estado;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var item = await _context.ReservasStock.FindAsync(id);
            if (item is null) return NotFound();
            
            _context.ReservasStock.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
