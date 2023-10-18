using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Migrations;

/// <inheritdoc />
public partial class UpdateFileManagement05 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_FileManagementFiles",
            table: "FileManagementFiles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_FileManagementFileContainers",
            table: "FileManagementFileContainers");

        migrationBuilder.DropPrimaryKey(
            name: "PK_FileManagementFileAccessTokens",
            table: "FileManagementFileAccessTokens");

        migrationBuilder.RenameTable(
            name: "FileManagementFiles",
            newName: "FmFiles");

        migrationBuilder.RenameTable(
            name: "FileManagementFileContainers",
            newName: "FmFileContainers");

        migrationBuilder.RenameTable(
            name: "FileManagementFileAccessTokens",
            newName: "FmFileAccessTokens");

        migrationBuilder.RenameIndex(
            name: "IX_FileManagementFiles_UniqueId",
            newName: "IX_FmFiles_UniqueId",
            table: "FmFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FileManagementFiles_Hash",
            newName: "IX_FmFiles_Hash",
            table: "FmFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FileManagementFiles_FileName",
            newName: "IX_FmFiles_FileName",
            table: "FmFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FileManagementFiles_CreationTime",
            newName: "IX_FmFiles_CreationTime",
            table: "FmFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FileManagementFiles_ContainerId",
            newName: "IX_FmFiles_ContainerId",
            table: "FmFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FileManagementFileContainers_Name",
            newName: "IX_FmFileContainers_Name",
            table: "FmFileContainers");

        migrationBuilder.RenameIndex(
            name: "IX_FileManagementFileAccessTokens_Token",
            newName: "IX_FmFileAccessTokens_Token",
            table: "FmFileAccessTokens");

        migrationBuilder.RenameIndex(
            name: "IX_FileManagementFileAccessTokens_FileId",
            newName: "IX_FmFileAccessTokens_FileId",
            table: "FmFileAccessTokens");

        migrationBuilder.AddPrimaryKey(
            name: "PK_FmFiles",
            table: "FmFiles",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_FmFileContainers",
            table: "FmFileContainers",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_FmFileAccessTokens",
            table: "FmFileAccessTokens",
            column: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_FmFiles",
            table: "FmFiles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_FmFileContainers",
            table: "FmFileContainers");

        migrationBuilder.DropPrimaryKey(
            name: "PK_FmFileAccessTokens",
            table: "FmFileAccessTokens");

        migrationBuilder.RenameTable(
            name: "FmFiles",
            newName: "FileManagementFiles");

        migrationBuilder.RenameTable(
            name: "FmFileContainers",
            newName: "FileManagementFileContainers");

        migrationBuilder.RenameTable(
            name: "FmFileAccessTokens",
            newName: "FileManagementFileAccessTokens");

        migrationBuilder.RenameIndex(
            name: "IX_FmFiles_UniqueId",
            newName: "IX_FileManagementFiles_UniqueId",
            table: "FileManagementFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FmFiles_Hash",
            newName: "IX_FileManagementFiles_Hash",
            table: "FileManagementFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FmFiles_FileName",
            newName: "IX_FileManagementFiles_FileName",
            table: "FileManagementFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FmFiles_CreationTime",
            newName: "IX_FileManagementFiles_CreationTime",
            table: "FileManagementFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FmFiles_ContainerId",
            newName: "IX_FileManagementFiles_ContainerId",
            table: "FileManagementFiles");

        migrationBuilder.RenameIndex(
            name: "IX_FmFileContainers_Name",
            newName: "IX_FileManagementFileContainers_Name",
            table: "FileManagementFileContainers");

        migrationBuilder.RenameIndex(
            name: "IX_FmFileAccessTokens_Token",
            newName: "IX_FileManagementFileAccessTokens_Token",
            table: "FileManagementFileAccessTokens");

        migrationBuilder.RenameIndex(
            name: "IX_FmFileAccessTokens_FileId",
            newName: "IX_FileManagementFileAccessTokens_FileId",
            table: "FileManagementFileAccessTokens");

        migrationBuilder.AddPrimaryKey(
            name: "PK_FileManagementFiles",
            table: "FileManagementFiles",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_FileManagementFileContainers",
            table: "FileManagementFileContainers",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_FileManagementFileAccessTokens",
            table: "FileManagementFileAccessTokens",
            column: "Id");
    }
}
