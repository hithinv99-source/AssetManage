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
    public class IssuesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ISpService _sp;

        public IssuesController(ApplicationDbContext db, ISpService sp)
        {
            _db = db;
            _sp = sp;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Set<Issue>().ToListAsync());

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Employee,Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(IssueCreateDto dto)
        {
            // call stored proc for logging issue
            await _sp.LogIssueAsync(dto.AssetID, dto.ReportedByUserID, dto.Description ?? string.Empty, dto.RequiresRepair);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IssueUpdateDto dto)
        {
            var issue = await _db.Set<Issue>().FindAsync(id);
            if (issue == null) return NotFound();

            issue.Description = dto.Description;
            issue.Status = dto.Status;
            issue.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var issue = await _db.Set<Issue>().FindAsync(id);
            if (issue == null) return NotFound();

            issue.Status = AssetManage.Models.Enums.IssueStatus.Closed;
            issue.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
