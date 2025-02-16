using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Migrations;

/// <inheritdoc />
public partial class Update_FileManagement_06 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FmFileContainerAccesses");

        migrationBuilder.DropTable(
            name: "FmFileAccessTokens");

        migrationBuilder.DropTable(
            name: "FmFileContainers");

        migrationBuilder.DropTable(
            name: "FmFileTags");

        migrationBuilder.DropTable(
            name: "FmFiles");

        migrationBuilder.CreateTable(
            name: "AppFileAccessTokens",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Length = table.Column<long>(type: "bigint", nullable: false),
                MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                Token = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                DownloadCount = table.Column<long>(type: "bigint", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_AppFileAccessTokens", x => x.Id));

        migrationBuilder.CreateTable(
            name: "AppFileContainers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                AccessMode = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                MaximumEachFileSize = table.Column<long>(type: "bigint", nullable: false),
                MaximumFileQuantity = table.Column<int>(type: "int", nullable: false),
                OverrideBehavior = table.Column<int>(type: "int", nullable: false),
                AllowAnyFileExtension = table.Column<bool>(type: "bit", nullable: false),
                AllowedFileExtensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ProhibitedFileExtensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AutoDeleteBlob = table.Column<bool>(type: "bit", nullable: false),
                FilesCount = table.Column<int>(type: "int", nullable: false),
                EntityVersion = table.Column<int>(type: "int", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_AppFileContainers", x => x.Id));

        migrationBuilder.CreateTable(
            name: "AppFileItems",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDirectory = table.Column<bool>(type: "bit", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                UniqueId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                Length = table.Column<long>(type: "bigint", nullable: false),
                BlobName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Hash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                EntityVersion = table.Column<int>(type: "int", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_AppFileItems", x => x.Id));

        migrationBuilder.CreateTable(
            name: "AppFileContainerAccesses",
            columns: table => new
            {
                FileContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProviderName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                List = table.Column<bool>(type: "bit", nullable: false),
                Read = table.Column<bool>(type: "bit", nullable: false),
                Write = table.Column<bool>(type: "bit", nullable: false),
                Delete = table.Column<bool>(type: "bit", nullable: false),
                Overwrite = table.Column<bool>(type: "bit", nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppFileContainerAccesses", x => new { x.FileContainerId, x.ProviderName, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AppFileContainerAccesses_AppFileContainers_FileContainerId",
                    column: x => x.FileContainerId,
                    principalTable: "AppFileContainers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AppFilePaths",
            columns: table => new
            {
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FullPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppFilePaths", x => x.FileId);
                table.ForeignKey(
                    name: "FK_AppFilePaths_AppFileItems_FileId",
                    column: x => x.FileId,
                    principalTable: "AppFileItems",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AppFileTags",
            columns: table => new
            {
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppFileTags", x => new { x.FileId, x.Name });
                table.ForeignKey(
                    name: "FK_AppFileTags_AppFileItems_FileId",
                    column: x => x.FileId,
                    principalTable: "AppFileItems",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AppFileAccessTokens_FileId",
            table: "AppFileAccessTokens",
            column: "FileId");

        migrationBuilder.CreateIndex(
            name: "IX_AppFileAccessTokens_Token",
            table: "AppFileAccessTokens",
            column: "Token");

        migrationBuilder.CreateIndex(
            name: "IX_AppFileContainers_Name",
            table: "AppFileContainers",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_AppFileItems_ContainerId",
            table: "AppFileItems",
            column: "ContainerId");

        migrationBuilder.CreateIndex(
            name: "IX_AppFileItems_CreationTime",
            table: "AppFileItems",
            column: "CreationTime",
            descending: new bool[0]);

        migrationBuilder.CreateIndex(
            name: "IX_AppFileItems_FileName",
            table: "AppFileItems",
            column: "FileName");

        migrationBuilder.CreateIndex(
            name: "IX_AppFileItems_Hash",
            table: "AppFileItems",
            column: "Hash");

        migrationBuilder.CreateIndex(
            name: "IX_AppFileItems_UniqueId",
            table: "AppFileItems",
            column: "UniqueId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AppFileAccessTokens");

        migrationBuilder.DropTable(
            name: "AppFileContainerAccesses");

        migrationBuilder.DropTable(
            name: "AppFilePaths");

        migrationBuilder.DropTable(
            name: "AppFileTags");

        migrationBuilder.DropTable(
            name: "AppFileContainers");

        migrationBuilder.DropTable(
            name: "AppFileItems");

        migrationBuilder.CreateTable(
            name: "FmFileAccessTokens",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DownloadCount = table.Column<long>(type: "bigint", nullable: false),
                ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Token = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_FmFileAccessTokens", x => x.Id));

        migrationBuilder.CreateTable(
            name: "FmFileContainers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AccessMode = table.Column<int>(type: "int", nullable: false),
                AllowAnyFileExtension = table.Column<bool>(type: "bit", nullable: false),
                AllowedFileExtensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AutoDeleteBlob = table.Column<bool>(type: "bit", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FilesCount = table.Column<int>(type: "int", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                MaximumEachFileSize = table.Column<long>(type: "bigint", nullable: false),
                MaximumFileQuantity = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                OverrideBehavior = table.Column<int>(type: "int", nullable: false),
                ProhibitedFileExtensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_FmFileContainers", x => x.Id));

        migrationBuilder.CreateTable(
            name: "FmFiles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BlobName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Hash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                IsDirectory = table.Column<bool>(type: "bit", nullable: false),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Length = table.Column<long>(type: "bigint", nullable: false),
                MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UniqueId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_FmFiles", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_FmFileAccessTokens_FileId",
            table: "FmFileAccessTokens",
            column: "FileId");

        migrationBuilder.CreateIndex(
            name: "IX_FmFileAccessTokens_Token",
            table: "FmFileAccessTokens",
            column: "Token");

        migrationBuilder.CreateIndex(
            name: "IX_FmFileContainers_Name",
            table: "FmFileContainers",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_FmFiles_ContainerId",
            table: "FmFiles",
            column: "ContainerId");

        migrationBuilder.CreateIndex(
            name: "IX_FmFiles_CreationTime",
            table: "FmFiles",
            column: "CreationTime",
            descending: new bool[0]);

        migrationBuilder.CreateIndex(
            name: "IX_FmFiles_FileName",
            table: "FmFiles",
            column: "FileName");

        migrationBuilder.CreateIndex(
            name: "IX_FmFiles_Hash",
            table: "FmFiles",
            column: "Hash");

        migrationBuilder.CreateIndex(
            name: "IX_FmFiles_UniqueId",
            table: "FmFiles",
            column: "UniqueId");
    }
}
