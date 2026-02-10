using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssetManage.Models.Enums;

namespace AssetManage.Models.Entities
{
    [Table("t_assets")]
    public class Asset
    {
        [Key]
        public int AssetID { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }

        public string? Tag { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public decimal? Cost { get; set; }

        public AssetStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
