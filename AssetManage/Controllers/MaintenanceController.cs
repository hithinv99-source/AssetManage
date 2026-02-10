using AssetManage.Data;
using AssetManage.DTOs;
using AssetManage.Models.Entities;
using AssetManage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ISpService _sp;

        public MaintenanceController(ApplicationDbContext db, ISpService sp)
        {
            _db = db;
            _sp = sp;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Set<Maintenance>().ToListAsync());

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(MaintenanceCreateDto dto)
        {
            var m = new Maintenance { AssetID = dto.AssetID, Description = dto.Description, ScheduleDate = dto.ScheduleDate, Status = AssetManage.Models.Enums.MaintenanceStatus.Scheduled, CreatedAt = DateTime.UtcNow };
            _db.Set<Maintenance>().Add(m);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = m.MaintenanceID }, m);
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Manager")]
        [HttpPost("complete/{id}")]
        public async Task<IActionResult> Complete(int id, [FromBody] DateTime completedDate)
        {
            await _sp.CompleteMaintenanceAsync(id, completedDate);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MaintenanceUpdateDto dto)
        {
            var m = await _db.Set<Maintenance>().FindAsync(id);
            if (m == null) return NotFound();

            m.Description = dto.Description;
            m.ScheduleDate = dto.ScheduleDate;
            m.CompletedDate = dto.CompletedDate;
            m.Status = dto.Status;
            m.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Set<Maintenance>().FindAsync(id);
            if (m == null) return NotFound();

            m.Status = AssetManage.Models.Enums.MaintenanceStatus.Cancelled;
            m.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
