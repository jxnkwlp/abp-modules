using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Migrations;

/// <inheritdoc />
public partial class AddIdentityClientManagement : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PassingwindIdentityClients",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                ProviderType = table.Column<int>(type: "int", nullable: false),
                ProviderName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                DisplayOrder = table.Column<int>(type: "int", nullable: false),
                IsDebugMode = table.Column<bool>(type: "bit", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RequiredClaimTypes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_PassingwindIdentityClients", x => x.Id));

        migrationBuilder.CreateTable(
            name: "PassingwindIdentityClientClaimMaps",
            columns: table => new
            {
                IdentityClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Action = table.Column<int>(type: "int", nullable: false),
                ValueFromType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                RawValue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PassingwindIdentityClientClaimMaps", x => new { x.IdentityClientId, x.ClaimType });
                table.ForeignKey(
                    name: "FK_PassingwindIdentityClientClaimMaps_PassingwindIdentityClients_IdentityClientId",
                    column: x => x.IdentityClientId,
                    principalTable: "PassingwindIdentityClients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PassingwindIdentityClientConfigurations",
            columns: table => new
            {
                IdentityClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PassingwindIdentityClientConfigurations", x => new { x.IdentityClientId, x.Name });
                table.ForeignKey(
                    name: "FK_PassingwindIdentityClientConfigurations_PassingwindIdentityClients_IdentityClientId",
                    column: x => x.IdentityClientId,
                    principalTable: "PassingwindIdentityClients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PassingwindIdentityClients_CreationTime",
            table: "PassingwindIdentityClients",
            column: "CreationTime",
            descending: new bool[0]);

        migrationBuilder.CreateIndex(
            name: "IX_PassingwindIdentityClients_Name",
            table: "PassingwindIdentityClients",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_PassingwindIdentityClients_ProviderName",
            table: "PassingwindIdentityClients",
            column: "ProviderName");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PassingwindIdentityClientClaimMaps");

        migrationBuilder.DropTable(
            name: "PassingwindIdentityClientConfigurations");

        migrationBuilder.DropTable(
            name: "PassingwindIdentityClients");
    }
}
