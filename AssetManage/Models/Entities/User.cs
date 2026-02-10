using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssetManage.Models.Enums;

namespace AssetManage.Models.Entities
{
    [Table("t_users")]
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        public int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public Role? Role { get; set; }

        public string? Department { get; set; }

        public string? PasswordHash { get; set; }

        public UserStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
