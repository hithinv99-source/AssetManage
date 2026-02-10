using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManage.Models.Entities
{
    [Table("t_roles")]
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
