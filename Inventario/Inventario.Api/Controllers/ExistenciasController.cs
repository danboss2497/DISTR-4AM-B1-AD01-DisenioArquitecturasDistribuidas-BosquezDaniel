using Inventario.Api.Data;
using Inventario.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExistenciasController : ControllerBase
    {
        private readonly InventarioDbContext _context;
        public ExistenciasController(InventarioDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Existencia>>> Obtener()
            => Ok(await _context.Existencias.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Existencia>> ObtenerPorId(int id)
        {
            var item = await _context.Existencias.AsNoTracking().FirstOrDefaultAsync(x => x.IdExistencia == id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Existencia>> Crear(Existencia item)
        {
            if (!await _context.Bodegas.AnyAsync(x => x.IdBodega == item.IdBodega))
                return BadRequest("La bodega no existe.");
            if (await _context.Existencias.AnyAsync(x => x.IdProducto == item.IdProducto && x.IdBodega == item.IdBodega))
                return Conflict("Ya existe stock de ese producto en la bodega.");
            _context.Existencias.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = item.IdExistencia }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, Existencia item)
        {
            if (id != item.IdExistencia) return BadRequest("El ID de la ruta no coincide.");
            var actual = await _context.Existencias.FindAsync(id);
            if (actual is null) return NotFound();
            if (!await _context.Bodegas.AnyAsync(x => x.IdBodega == item.IdBodega))
                return BadRequest("La bodega no existe.");
            if (await _context.Existencias.AnyAsync(x =>
                x.IdProducto == item.IdProducto && x.IdBodega == item.IdBodega && x.IdExistencia != id))
                return Conflict("Ya existe stock de ese producto en la bodega.");
            actual.IdProducto = item.IdProducto;
            actual.IdBodega = item.IdBodega;
            actual.Stock = item.Stock;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var item = await _context.Existencias.FindAsync(id);
            if (item is null) return NotFound();
            
            _context.Existencias.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
