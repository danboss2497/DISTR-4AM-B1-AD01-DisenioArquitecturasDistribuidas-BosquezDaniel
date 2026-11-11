using Catalogo.Api.Data;
using Catalogo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly CatalogoDbContext _context;

        public CategoriasController(CatalogoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> ObtenerCategorias()
        {
            var categorias = await _context.Categorias
                .AsNoTracking()
                .ToListAsync();

            return Ok(categorias);

        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> CrearCategoria(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ObtenerCategoriaPorId),
                new { id = categoria.IdCategoria },
                categoria);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Categoria>> ObtenerCategoriaPorId(int id)
        {
            var categoria = await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria is null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarCategoria(int id, Categoria categoria)
        {
            if (id != categoria.IdCategoria)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la categoría.");
            }

            var categoriaExistente = await _context.Categorias.FindAsync(id);

            if (categoriaExistente is null)
            {
                return NotFound();
            }

            categoriaExistente.Nombre = categoria.Nombre;
            categoriaExistente.Descripcion = categoria.Descripcion;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria is null)
            {
                return NotFound();
            }

            var tieneProductos = await _context.Productos
                .AnyAsync(producto => producto.IdCategoria == id);

            if (tieneProductos)
            {
                return Conflict("No se puede eliminar la categoría porque tiene productos asociados.");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}