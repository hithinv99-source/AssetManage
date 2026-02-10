using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssetManage.Models.Enums;

namespace AssetManage.Models.Entities
{
    [Table("t_issues")]
    public class Issue
    {
        [Key]
        public int IssueID { get; set; }

        public int AssetID { get; set; }

        [ForeignKey("AssetID")]
        public Asset? Asset { get; set; }

        public int ReportedByUserID { get; set; }

        [ForeignKey("ReportedByUserID")]
        public User? ReportedBy { get; set; }

        public string? Description { get; set; }

        public DateTime ReportedDate { get; set; }

        public IssueStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
