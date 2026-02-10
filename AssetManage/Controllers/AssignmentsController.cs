using AssetManage.DTOs;
using AssetManage.Services;
using Microsoft.AspNetCore.Mvc;
namespace AssetManage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private readonly ISpService _spService;

        private readonly AssetManage.Data.ApplicationDbContext _db;

        public AssignmentsController(ISpService spService, AssetManage.Data.ApplicationDbContext db)
        {
            _spService = spService;
            _db = db;
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Manager")]
        [HttpPost("assign")]
        public async Task<IActionResult> Assign(AssignAssetDto dto)
        {
            await _spService.AssignAssetAsync(dto.AssetID, dto.AssignedToUserID, dto.AssignedDate, dto.Location ?? string.Empty);
            return Ok();
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Manager")]
        [HttpPost("return")]
        public async Task<IActionResult> Return(ReturnAssetDto dto)
        {
            await _spService.ReturnAssetAsync(dto.AssignmentID, dto.ReturnDate);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AssignmentResponseDto dto)
        {
            var a = await _db.Set<AssetManage.Models.Entities.Assignment>().FindAsync(id);
            if (a == null) return NotFound();

            a.ReturnDate = dto.ReturnDate;
            a.Status = dto.Status;
            a.Location = dto.Location;
            a.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.Set<AssetManage.Models.Entities.Assignment>().FindAsync(id);
            if (a == null) return NotFound();

            a.Status = AssetManage.Models.Enums.AssignmentStatus.Deleted;
            a.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
