using AssetManage.Models.Enums;

namespace AssetManage.DTOs
{
    // UserCreateDto is not used; user creation is handled by AuthController.Register
    public record UserUpdateDto(string Name, string Email, int RoleID, string? Department, UserStatus Status);
    public record UserResponseDto(int UserID, string Name, string Email, int RoleID, string? Department, UserStatus Status);

    public record UserRegisterDto(string Name, string Email, int RoleID, string? Department, string Password);
    public record UserLoginDto(string Email, string Password);
}
