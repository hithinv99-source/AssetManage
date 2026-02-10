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
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _db.Set<User>().ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _db.Set<User>().FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // User creation is handled exclusively by AuthController.Register

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto dto)
        {
            var user = await _db.Set<User>().FindAsync(id);
            if (user == null) return NotFound();

            // Do not allow password updates here; handled only by AuthController
            user.Name = dto.Name;
            user.Email = dto.Email;
            user.RoleID = dto.RoleID;
            user.Department = dto.Department;
            user.Status = dto.Status;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Set<User>().FindAsync(id);
            if (user == null) return NotFound();

            user.Status = AssetManage.Models.Enums.UserStatus.Deleted;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
