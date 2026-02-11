using AssetManage.Models.Enums;

namespace AssetManage.DTOs
{
    public record AssetCreateDto(string Name, int CategoryID, string? Tag, DateTime? PurchaseDate, decimal? Cost);
    public record AssetUpdateDto(string Name, int CategoryID, string? Tag, DateTime? PurchaseDate, decimal? Cost, AssetStatus Status);
    public record AssetResponseDto(int AssetID, string Name, int CategoryID, string CategoryName, string? Tag, DateTime? PurchaseDate, decimal? Cost, AssetStatus Status);
}