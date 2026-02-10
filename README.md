# AssetManage

This is an ASP.NET Core (.NET 8) Web API using EF Core (Code First) and SQL Server. The repository includes model classes, Fluent API configuration, and EF Core migrations for tables and stored procedures.

Pre-requisites
- .NET 8 SDK installed
- SQL Server or LocalDB available
- Optional: `dotnet-ef` CLI tool (used below)

Default connection
- The app reads the connection string named `DefaultConnection` from configuration. If not present, `Program.cs` falls back to:

  `Server=(localdb)\\mssqllocaldb;Database=AssetManageDb;Trusted_Connection=True;`

Apply migrations and create database (local)
1. Install `dotnet-ef` if not installed:
   - Global tool (recommended):
     `dotnet tool install --global dotnet-ef`
   - Or use `dotnet tool restore` if you prefer a manifest-based tool.

2. From the solution root run:

   dotnet ef database update --project AssetManage --startup-project AssetManage

   This will apply the included migrations (InitialCreate and CreateStoredProcedures) and create the database, tables, and stored procedures.

Notes
- Migrations included in the repository:
  - `Migrations/20260101000000_InitialCreate.cs` — creates tables mapped to `t_...` names.
  - `Migrations/CreateStoredProcedures.cs` — creates stored procedures: `sp_AssignAsset`, `sp_ReturnAsset`, `sp_LogIssue`, `sp_CompleteMaintenance`, `sp_GenerateReport`.

- If you need to change the database, update the connection string in `appsettings.json` or set the `DefaultConnection` environment variable before running migrations.

Run the app
- From solution root run:

  dotnet run --project AssetManage

- The API will start and Swagger will be available in Development environment at `/swagger`.

Adding new migrations
1. Add a migration:
   `dotnet ef migrations add <Name> --project AssetManage --startup-project AssetManage -o Migrations`
2. Apply migrations:
   `dotnet ef database update --project AssetManage --startup-project AssetManage`

Support
- If `dotnet-ef` is not available in your environment, install it as a global tool or run from Visual Studio Package Manager Console.

