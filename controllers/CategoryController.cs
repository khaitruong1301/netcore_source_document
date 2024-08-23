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
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CategoriesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories(string keyword = "")
        {
            keyword = SlugHelper.StringToSlug(keyword);
            var categories = await _context.Categories
                .Where(p => !p.Deleted && (string.IsNullOrEmpty(keyword) || p.Alias.Contains(keyword)))
                .Select(category => new CategoryViewModel
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    Alias = category.Alias,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                    Deleted = category.Deleted
                })
                .ToListAsync();

            return categories;
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryViewModel>> GetCategory(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id && !c.Deleted)
                .Select(category => new CategoryViewModel
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    Alias = category.Alias,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                    Deleted = category.Deleted
                })
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryViewModel>> PostCategory(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Các định dạng ngày hợp lệ
            string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy" };

            // Chuyển đổi CreatedAt từ chuỗi ngày sang DateTime
            if (!string.IsNullOrEmpty(categoryViewModel.CreatedAtString))
            {
                if (!DateTime.TryParseExact(categoryViewModel.CreatedAtString, dateFormats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime parsedDate))
                {
                    ModelState.AddModelError("CreatedAt", "Ngày không hợp lệ. Vui lòng sử dụng định dạng dd/MM/yyyy hoặc dd-MM-yyyy.");
                    return BadRequest(ModelState);
                }

                categoryViewModel.CreatedAt = parsedDate;
            }
            else
            {
                categoryViewModel.CreatedAt = DateTime.UtcNow; // Giá trị mặc định nếu không có ngày
            }

            var category = new Category
            {
                CategoryName = categoryViewModel.CategoryName,
                CreatedAt = categoryViewModel.CreatedAt,
                Deleted = false
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            categoryViewModel.Id = category.Id;

            return CreatedAtAction("GetCategory", new { id = category.Id }, categoryViewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryViewModel categoryViewModel)
        {
            if (id != categoryViewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null || category.Deleted)
            {
                return NotFound();
            }
             // Các định dạng ngày hợp lệ
            string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy" };
            // Chuyển đổi UpdatedAt từ chuỗi ngày sang DateTime nếu có
            if (!string.IsNullOrEmpty(categoryViewModel.UpdatedAtString))
            {
                if (!DateTime.TryParseExact(categoryViewModel.UpdatedAtString, dateFormats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime parsedDate))
                {
                    ModelState.AddModelError("UpdatedAt", "Ngày không hợp lệ. Vui lòng sử dụng định dạng dd/MM/yyyy hoặc dd-MM-yyyy.");
                    return BadRequest(ModelState);
                }

                category.UpdatedAt = parsedDate;
            }
            else
            {
                category.UpdatedAt = DateTime.UtcNow; // Giá trị mặc định nếu không có ngày
            }

            category.CategoryName = categoryViewModel.CategoryName;

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null || category.Deleted)
            {
                return NotFound();
            }

            category.Deleted = true;
            category.UpdatedAt = DateTime.UtcNow;
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
