using AssetManage.Data;
using AssetManage.DTOs;
using AssetManage.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Set<Category>().ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            var c = new Category { Name = dto.Name, CreatedAt = DateTime.UtcNow };
            _db.Set<Category>().Add(c);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = c.CategoryID }, c);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            var c = await _db.Set<Category>().FindAsync(id);
            if (c == null) return NotFound();

            c.Name = dto.Name;
            c.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Set<Category>().FindAsync(id);
            if (c == null) return NotFound();

            c.UpdatedAt = DateTime.UtcNow; // soft-delete via UpdatedAt/status not present
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
