using AssetManage.Models.Enums;

namespace AssetManage.DTOs
{
    public record IssueCreateDto(int AssetID, int ReportedByUserID, string? Description, bool RequiresRepair);
    public record IssueUpdateDto(string? Description, IssueStatus Status);
    public record IssueResponseDto(int IssueID, int AssetID, int ReportedByUserID, string? Description, DateTime ReportedDate, IssueStatus Status);
}
