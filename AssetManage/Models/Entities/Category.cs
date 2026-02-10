using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManage.Models.Entities
{
    [Table("t_categories")]
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
