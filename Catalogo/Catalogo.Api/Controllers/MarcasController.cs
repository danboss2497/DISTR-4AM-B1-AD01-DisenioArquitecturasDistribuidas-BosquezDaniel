using Catalogo.Api.Data;
using Catalogo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasController : ControllerBase
    {
        private readonly CatalogoDbContext _context;

        public MarcasController(CatalogoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marca>>> ObtenerMarcas()
        {
            var marcas = await _context.Marcas
                .AsNoTracking()
                .ToListAsync();

            return Ok(marcas);
        }
        [HttpPost]
        public async Task<ActionResult<Marca>> CrearMarca(Marca marca)
        {
            _context.Marcas.Add(marca);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ObtenerMarcaPorId),
                new { id = marca.IdMarca },
                marca);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Marca>> ObtenerMarcaPorId(int id)
        {
            var marca = await _context.Marcas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdMarca == id);

            if (marca is null)
            {
                return NotFound();
            }

            return Ok(marca);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarMarca(int id, Marca marca)
        {
            if (id != marca.IdMarca)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la marca.");
            }

            var marcaExistente = await _context.Marcas.FindAsync(id);

            if (marcaExistente is null)
            {
                return NotFound();
            }

            marcaExistente.Nombre = marca.Nombre;
            marcaExistente.PaisOrigen = marca.PaisOrigen;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarMarca(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);

            if (marca is null)
            {
                return NotFound();
            }

            var tieneProductos = await _context.Productos
                .AnyAsync(producto => producto.IdMarca == id);

            if (tieneProductos)
            {
                return Conflict("No se puede eliminar la marca porque tiene productos asociados.");
            }

            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}