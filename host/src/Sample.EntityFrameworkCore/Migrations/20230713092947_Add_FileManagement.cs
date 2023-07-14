using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Migrations
{
    /// <inheritdoc />
    public partial class AddFileManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileManagementFileContainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AccessMode = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaximumEachFileSize = table.Column<long>(type: "bigint", nullable: false),
                    MaximumFileQuantity = table.Column<int>(type: "int", nullable: false),
                    OverrideBehavior = table.Column<int>(type: "int", nullable: false),
                    AllowAnyFileExtension = table.Column<bool>(type: "bit", nullable: false),
                    AllowedFileExtensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProhibitedFileExtensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilesCount = table.Column<int>(type: "int", nullable: false),
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
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManagementFileContainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileManagementFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDirectory = table.Column<bool>(type: "bit", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UniqueId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    BlobName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
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
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManagementFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFileContainers_Name",
                table: "FileManagementFileContainers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_ContainerId",
                table: "FileManagementFiles",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_FileName",
                table: "FileManagementFiles",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_Hash",
                table: "FileManagementFiles",
                column: "Hash");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_UniqueId",
                table: "FileManagementFiles",
                column: "UniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileManagementFileContainers");

            migrationBuilder.DropTable(
                name: "FileManagementFiles");
        }
    }
}
