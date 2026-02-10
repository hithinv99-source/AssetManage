using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace AssetManage.Migrations
{
    [DbContext(typeof(AssetManage.Data.ApplicationDbContext))]
    [Migration("20260206000000_EmptyMigration")]
    public partial class EmptyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
   IF OBJECT_ID('sp_AssignAsset', 'P') IS NOT NULL
       DROP PROCEDURE sp_AssignAsset;
   EXEC('
   CREATE PROCEDURE sp_AssignAsset
       @AssetID INT,
       @AssignedToUserID INT,
       @AssignedDate DATETIME2,
       @Location NVARCHAR(200)
   AS
   BEGIN
       SET NOCOUNT ON;
       IF NOT EXISTS (
           SELECT 1 FROM t_assets
           WHERE AssetID = @AssetID AND Status = ''Available''
       )
       BEGIN
           RAISERROR(''Asset is not available'', 16, 1);
           RETURN;
       END
       IF EXISTS (
           SELECT 1 FROM t_assignments
           WHERE AssetID = @AssetID AND Status = ''Active''
       )
       BEGIN
           RAISERROR(''Asset already has an active assignment'', 16, 1);
           RETURN;
       END
       INSERT INTO t_assignments
           (AssetID, AssignedToUserID, AssignedDate, Status, Location, CreatedAt)
       VALUES
           (@AssetID, @AssignedToUserID, @AssignedDate, ''Active'', @Location, SYSUTCDATETIME());
       UPDATE t_assets
       SET Status = ''Assigned'', UpdatedAt = SYSUTCDATETIME()
       WHERE AssetID = @AssetID;
   END
   ');
   ");
            migrationBuilder.Sql(@"
   IF OBJECT_ID('sp_ReturnAsset', 'P') IS NOT NULL
       DROP PROCEDURE sp_ReturnAsset;
   EXEC('
   CREATE PROCEDURE sp_ReturnAsset
       @AssignmentID INT,
       @ReturnDate DATETIME2
   AS
   BEGIN
       SET NOCOUNT ON;
       UPDATE t_assignments
       SET Status = ''Returned'',
           ReturnDate = @ReturnDate,
           UpdatedAt = SYSUTCDATETIME()
       WHERE AssignmentID = @AssignmentID;
       DECLARE @AssetID INT =
           (SELECT AssetID FROM t_assignments WHERE AssignmentID = @AssignmentID);
       UPDATE t_assets
       SET Status = ''Available'', UpdatedAt = SYSUTCDATETIME()
       WHERE AssetID = @AssetID;
   END
   ');
   ");
            migrationBuilder.Sql(@"
   IF OBJECT_ID('sp_LogIssue', 'P') IS NOT NULL
       DROP PROCEDURE sp_LogIssue;
   EXEC('
   CREATE PROCEDURE sp_LogIssue
       @AssetID INT,
       @ReportedByUserID INT,
       @Description NVARCHAR(MAX),
       @RequiresRepair BIT
   AS
   BEGIN
       SET NOCOUNT ON;
       INSERT INTO t_issues
           (AssetID, ReportedByUserID, Description, ReportedDate, Status, CreatedAt)
       VALUES
           (@AssetID, @ReportedByUserID, @Description,
            SYSUTCDATETIME(), ''Open'', SYSUTCDATETIME());
       IF @RequiresRepair = 1
       BEGIN
           UPDATE t_assets
           SET Status = ''InRepair'', UpdatedAt = SYSUTCDATETIME()
           WHERE AssetID = @AssetID;
       END
   END
   ');
   ");
            migrationBuilder.Sql(@"
   IF OBJECT_ID('sp_CompleteMaintenance', 'P') IS NOT NULL
       DROP PROCEDURE sp_CompleteMaintenance;
   EXEC('
   CREATE PROCEDURE sp_CompleteMaintenance
       @MaintenanceID INT,
       @CompletedDate DATETIME2
   AS
   BEGIN
       SET NOCOUNT ON;
       UPDATE t_maintenance
       SET Status = ''Completed'',
           CompletedDate = @CompletedDate,
           UpdatedAt = SYSUTCDATETIME()
       WHERE MaintenanceID = @MaintenanceID;
       DECLARE @AssetID INT =
           (SELECT AssetID FROM t_maintenance WHERE MaintenanceID = @MaintenanceID);
       UPDATE t_assets
       SET Status = ''Available'', UpdatedAt = SYSUTCDATETIME()
       WHERE AssetID = @AssetID;
   END
   ');
   ");
            migrationBuilder.Sql(@"
   IF OBJECT_ID('sp_GenerateReport', 'P') IS NOT NULL
       DROP PROCEDURE sp_GenerateReport;
   EXEC('
   CREATE PROCEDURE sp_GenerateReport
       @Scope NVARCHAR(200)
   AS
   BEGIN
       SET NOCOUNT ON;
       DECLARE @Metrics NVARCHAR(MAX);
       SELECT @Metrics = (
           SELECT
               (SELECT COUNT(*) FROM t_assets) AS TotalAssets,
               (SELECT COUNT(*) FROM t_assets WHERE Status = ''Available'') AS AvailableAssets,
               (SELECT COUNT(*) FROM t_assignments WHERE Status = ''Active'') AS ActiveAssignments,
               (SELECT COUNT(*) FROM t_issues WHERE Status = ''Open'') AS OpenIssues
           FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
       );
       INSERT INTO t_reports (Scope, Metrics, GeneratedDate, CreatedAt)
       VALUES (@Scope, @Metrics, SYSUTCDATETIME(), SYSUTCDATETIME());
   END
   ');
   ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_AssignAsset;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ReturnAsset;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_LogIssue;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_CompleteMaintenance;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GenerateReport;");
        }
    }
}
