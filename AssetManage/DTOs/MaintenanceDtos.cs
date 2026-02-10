using AssetManage.Models.Enums;

namespace AssetManage.DTOs
{
    public record MaintenanceCreateDto(int AssetID, string? Description, DateTime? ScheduleDate);
    public record MaintenanceUpdateDto(string? Description, DateTime? ScheduleDate, DateTime? CompletedDate, MaintenanceStatus Status);
    public record MaintenanceResponseDto(int MaintenanceID, int AssetID, string? Description, DateTime? ScheduleDate, DateTime? CompletedDate, MaintenanceStatus Status);
}
