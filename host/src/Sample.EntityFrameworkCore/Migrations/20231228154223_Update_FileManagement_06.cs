using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Sample.Migrations;

/// <inheritdoc />
public partial class UpdateFileManagement06 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "EntityVersion",
            table: "FmFiles",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "EntityVersion",
            table: "FmFileContainers",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<Guid>(
            name: "ContainerId",
            table: "FmFileAccessTokens",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<string>(
            name: "FileName",
            table: "FmFileAccessTokens",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<long>(
            name: "Length",
            table: "FmFileAccessTokens",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.AddColumn<string>(
            name: "MimeType",
            table: "FmFileAccessTokens",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            table: "FmFileAccessTokens",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "FmFileContainerAccesses",
            columns: table => new
            {
                FileContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProviderName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Read = table.Column<bool>(type: "bit", nullable: false),
                Write = table.Column<bool>(type: "bit", nullable: false),
                Delete = table.Column<bool>(type: "bit", nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FmFileContainerAccesses", x => new { x.FileContainerId, x.ProviderName, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_FmFileContainerAccesses_FmFileContainers_FileContainerId",
                    column: x => x.FileContainerId,
                    principalTable: "FmFileContainers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FmFileTags",
            columns: table => new
            {
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Tags = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FmFileTags", x => new { x.FileId, x.Tags });
                table.ForeignKey(
                    name: "FK_FmFileTags_FmFiles_FileId",
                    column: x => x.FileId,
                    principalTable: "FmFiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FmFileContainerAccesses");

        migrationBuilder.DropTable(
            name: "FmFileTags");

        migrationBuilder.DropColumn(
            name: "EntityVersion",
            table: "FmFiles");

        migrationBuilder.DropColumn(
            name: "EntityVersion",
            table: "FmFileContainers");

        migrationBuilder.DropColumn(
            name: "ContainerId",
            table: "FmFileAccessTokens");

        migrationBuilder.DropColumn(
            name: "FileName",
            table: "FmFileAccessTokens");

        migrationBuilder.DropColumn(
            name: "Length",
            table: "FmFileAccessTokens");

        migrationBuilder.DropColumn(
            name: "MimeType",
            table: "FmFileAccessTokens");

        migrationBuilder.DropColumn(
            name: "TenantId",
            table: "FmFileAccessTokens");
    }
}
