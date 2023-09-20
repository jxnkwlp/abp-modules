using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Migrations;

/// <inheritdoc />
public partial class AddApiKey : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PassingwindApiKeys",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                Secret = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_PassingwindApiKeys", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_PassingwindApiKeys_CreationTime",
            table: "PassingwindApiKeys",
            column: "CreationTime",
            descending: new bool[0]);

        migrationBuilder.CreateIndex(
            name: "IX_PassingwindApiKeys_Name",
            table: "PassingwindApiKeys",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_PassingwindApiKeys_Secret",
            table: "PassingwindApiKeys",
            column: "Secret");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PassingwindApiKeys");
    }
}
