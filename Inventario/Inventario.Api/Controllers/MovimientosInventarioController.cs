using Inventario.Api.Data;
using Inventario.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosInventarioController : ControllerBase
    {
        private readonly InventarioDbContext _context;
        public MovimientosInventarioController(InventarioDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoInventario>>> Obtener()
            => Ok(await _context.MovimientosInventario.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovimientoInventario>> ObtenerPorId(int id)
        {
            var item = await _context.MovimientosInventario.AsNoTracking().FirstOrDefaultAsync(x => x.IdMovimiento == id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<MovimientoInventario>> Crear(MovimientoInventario item)
        {
            if (!await _context.Bodegas.AnyAsync(x => x.IdBodega == item.IdBodega))
                return BadRequest("La bodega no existe.");
            
            _context.MovimientosInventario.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = item.IdMovimiento }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, MovimientoInventario item)
        {
            if (id != item.IdMovimiento) return BadRequest("El ID de la ruta no coincide.");
            var actual = await _context.MovimientosInventario.FindAsync(id);
            if (actual is null) return NotFound();
            if (!await _context.Bodegas.AnyAsync(x => x.IdBodega == item.IdBodega))
                return BadRequest("La bodega no existe.");
            actual.IdProducto = item.IdProducto;
            actual.IdBodega = item.IdBodega;
            actual.Tipo = item.Tipo;
            actual.Cantidad = item.Cantidad;
            actual.Fecha = item.Fecha;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var item = await _context.MovimientosInventario.FindAsync(id);
            if (item is null) return NotFound();
            
            _context.MovimientosInventario.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
