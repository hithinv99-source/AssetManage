using AssetManage.Models.Enums;

namespace AssetManage.DTOs
{
    public record AssignAssetDto(int AssetID, int AssignedToUserID, DateTime AssignedDate, string? Location);
    public record ReturnAssetDto(int AssignmentID, DateTime ReturnDate);
    public record AssignmentResponseDto(int AssignmentID, int AssetID, int AssignedToUserID, DateTime AssignedDate, DateTime? ReturnDate, AssignmentStatus Status, string? Location);
}
