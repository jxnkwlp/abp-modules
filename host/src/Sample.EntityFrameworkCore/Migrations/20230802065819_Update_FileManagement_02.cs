using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Migrations;

/// <inheritdoc />
public partial class UpdateFileManagement02 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "AutoDeleteBlob",
            table: "FileManagementFileContainers",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.CreateTable(
            name: "FileManagementFileAccessTokens",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Token = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                DownloadCount = table.Column<long>(type: "bigint", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_FileManagementFileAccessTokens", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_FileManagementFiles_CreationTime",
            table: "FileManagementFiles",
            column: "CreationTime",
            descending: new bool[0]);

        migrationBuilder.CreateIndex(
            name: "IX_FileManagementFileAccessTokens_FileId",
            table: "FileManagementFileAccessTokens",
            column: "FileId");

        migrationBuilder.CreateIndex(
            name: "IX_FileManagementFileAccessTokens_Token",
            table: "FileManagementFileAccessTokens",
            column: "Token");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FileManagementFileAccessTokens");

        migrationBuilder.DropIndex(
            name: "IX_FileManagementFiles_CreationTime",
            table: "FileManagementFiles");

        migrationBuilder.DropColumn(
            name: "AutoDeleteBlob",
            table: "FileManagementFileContainers");
    }
}
