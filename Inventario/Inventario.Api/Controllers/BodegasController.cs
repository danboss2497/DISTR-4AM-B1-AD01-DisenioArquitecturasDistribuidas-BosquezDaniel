using Inventario.Api.Data;
using Inventario.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BodegasController : ControllerBase
    {
        private readonly InventarioDbContext _context;
        public BodegasController(InventarioDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bodega>>> Obtener()
            => Ok(await _context.Bodegas.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Bodega>> ObtenerPorId(int id)
        {
            var item = await _context.Bodegas.AsNoTracking().FirstOrDefaultAsync(x => x.IdBodega == id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Bodega>> Crear(Bodega item)
        {
            
            _context.Bodegas.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = item.IdBodega }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, Bodega item)
        {
            if (id != item.IdBodega) return BadRequest("El ID de la ruta no coincide.");
            var actual = await _context.Bodegas.FindAsync(id);
            if (actual is null) return NotFound();
            actual.Nombre = item.Nombre;
            actual.Ciudad = item.Ciudad;
            actual.Direccion = item.Direccion;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var item = await _context.Bodegas.FindAsync(id);
            if (item is null) return NotFound();
            if (await _context.Existencias.AnyAsync(x => x.IdBodega == id) ||
                await _context.MovimientosInventario.AnyAsync(x => x.IdBodega == id) ||
                await _context.ReservasStock.AnyAsync(x => x.IdBodega == id))
                return Conflict("La bodega tiene registros asociados.");
            _context.Bodegas.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
