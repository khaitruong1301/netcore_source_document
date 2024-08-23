using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.models;
using api.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ProductsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts(string keyword = "")
        {
            keyword = SlugHelper.StringToSlug(keyword);
            var products = await _context.Products
                .Where(p => !p.Deleted && (string.IsNullOrEmpty(keyword) || p.Alias.Contains(keyword)))
                .Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Alias = product.Alias,
                    Description = product.Description,
                    Price = product.Price,
                    
                    Deleted = product.Deleted
                })
                .ToListAsync();

            return products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id && !p.Deleted)
                .Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Alias = product.Alias,
                    Description = product.Description,
                    Price = product.Price,
                    
                    Deleted = product.Deleted
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null || product.Deleted)
            {
                return NotFound();
            }

            product.ProductName = productViewModel.ProductName;
            product.Alias = productViewModel.Alias;
            product.Description = productViewModel.Description;
            product.Price = productViewModel.Price;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductViewModel>> PostProduct(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                ProductName = productViewModel.ProductName,
                Alias = productViewModel.Alias,
                Description = productViewModel.Description,
                Price = productViewModel.Price,
                CreatedAt = DateTime.UtcNow,
                Deleted = false
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            productViewModel.Id = product.Id;

            return CreatedAtAction("GetProduct", new { id = product.Id }, productViewModel);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.Deleted)
            {
                return NotFound();
            }

            product.Deleted = true;
            product.UpdatedAt = DateTime.UtcNow;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
