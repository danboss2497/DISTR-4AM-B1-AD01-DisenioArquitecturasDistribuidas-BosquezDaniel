using Catalogo.Api.Data;
using Catalogo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly CatalogoDbContext _context;

        public ProductosController(CatalogoDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> ObtenerProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .AsNoTracking()
                .ToListAsync();

            return Ok(productos);
        }

        // GET: api/Productos/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Producto>> ObtenerProductoPorId(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto is null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> CrearProducto(Producto producto)
        {
            if (!await _context.Categorias
                .AnyAsync(c => c.IdCategoria == producto.IdCategoria))
            {
                return BadRequest("La categoría indicada no existe.");
            }

            if (!await _context.Marcas
                .AnyAsync(m => m.IdMarca == producto.IdMarca))
            {
                return BadRequest("La marca indicada no existe.");
            }

            if (await _context.Productos.AnyAsync(p => p.SKU == producto.SKU))
            {
                return Conflict("Ya existe un producto con ese SKU.");
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ObtenerProductoPorId),
                new { id = producto.IdProducto },
                producto);
        }

        // PUT: api/Productos/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del producto.");
            }

            var productoExistente = await _context.Productos.FindAsync(id);

            if (productoExistente is null)
            {
                return NotFound();
            }

            if (!await _context.Categorias
                .AnyAsync(c => c.IdCategoria == producto.IdCategoria))
            {
                return BadRequest("La categoría indicada no existe.");
            }

            if (!await _context.Marcas
                .AnyAsync(m => m.IdMarca == producto.IdMarca))
            {
                return BadRequest("La marca indicada no existe.");
            }

            if (await _context.Productos.AnyAsync(
                p => p.SKU == producto.SKU && p.IdProducto != id))
            {
                return Conflict("Ya existe otro producto con ese SKU.");
            }

            productoExistente.IdCategoria = producto.IdCategoria;
            productoExistente.IdMarca = producto.IdMarca;
            productoExistente.Nombre = producto.Nombre;
            productoExistente.SKU = producto.SKU;
            productoExistente.Precio = producto.Precio;
            productoExistente.Estado = producto.Estado;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto is null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}  