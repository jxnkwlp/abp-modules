using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Migrations;

/// <inheritdoc />
public partial class Add_PermissionManagement : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AbpDynamicPermissionGroups",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                TargetName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_AbpDynamicPermissionGroups", x => x.Id));

        migrationBuilder.CreateTable(
            name: "AbpDynamicPermissions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                TargetName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_AbpDynamicPermissions", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_AbpDynamicPermissionGroups_Name",
            table: "AbpDynamicPermissionGroups",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpDynamicPermissions_GroupId",
            table: "AbpDynamicPermissions",
            column: "GroupId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpDynamicPermissions_Name",
            table: "AbpDynamicPermissions",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpDynamicPermissions_ParentId",
            table: "AbpDynamicPermissions",
            column: "ParentId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AbpDynamicPermissionGroups");

        migrationBuilder.DropTable(
            name: "AbpDynamicPermissions");
    }
}
