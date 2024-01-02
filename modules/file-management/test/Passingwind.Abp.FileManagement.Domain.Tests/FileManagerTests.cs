using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Passingwind.Abp.FileManagement;

public class FileManagerTests : FileManagementDomainTestBase
{
    private readonly IFileManager _manager;

    public FileManagerTests()
    {
        _manager = GetRequiredService<IFileManager>();
    }

    [Fact]
    public async Task Test_IsExists_01()
    {
        // Arrange ;
        string container = TestContainerName;
        string fileName = Guid.NewGuid().ToString() + ".txt";
        Guid? parentId = null;

        // Act
        var result = await _manager.IsExistsAsync(
            container,
            fileName,
            parentId);

        // Assert
        result.ShouldBe(false);

        // Act
        result = await _manager.IsExistsAsync(
          Guid.Parse("eaf86134-7148-490d-a08f-1a6abddf9f30"),
          fileName,
          parentId);

        // Assert
        result.ShouldBe(false);
    }

    [Fact]
    public async Task Test_IsExists_02()
    {
        // Arrange ;
        var containerId = Guid.Parse("eaf86134-7148-490d-a08f-1a6abddf9f30");
        string fileName = Guid.NewGuid().ToString() + ".txt";
        Guid? parentId = null;

        // Act
        var entity = await _manager.SaveAsync(containerId, fileName, new byte[0]);

        // Assert
        entity.ShouldNotBeNull();

        // Assert
        (await _manager.IsExistsAsync(containerId, fileName, parentId)).ShouldBe(true);
        (await _manager.IsFileExistsAsync(containerId, fileName, parentId)).ShouldBe(true);
        (await _manager.IsDirectoryExistsAsync(containerId, fileName, parentId)).ShouldBe(false);
    }

    [Fact]
    public async Task Test_IsExists_03()
    {
        // Arrange ;
        var containerId = Guid.Parse("eaf86134-7148-490d-a08f-1a6abddf9f30");
        string fileName = Guid.NewGuid().ToString();
        Guid? parentId = null;

        // Act
        var entity = await _manager.CreateDirectoryAsync(containerId, fileName, parentId);

        // Assert
        entity.ShouldNotBeNull();

        // Assert
        (await _manager.IsExistsAsync(containerId, fileName, parentId)).ShouldBe(true);
        (await _manager.IsDirectoryExistsAsync(containerId, fileName, parentId)).ShouldBe(true);
        (await _manager.IsFileExistsAsync(containerId, fileName, parentId)).ShouldBe(false);
    }

    //[Fact]
    //public async Task ChangeFileNameAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    string newFileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.ChangeFileNameAsync(
    //        containerId,
    //        fileName,
    //        newFileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task ChangeFileNameAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    string newFileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.ChangeFileNameAsync(
    //        container,
    //        fileName,
    //        newFileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CheckAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    long length = 0;
    //    string? mimeType = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.CheckAsync(
    //        container,
    //        fileName,
    //        length,
    //        mimeType,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CheckAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    long length = 0;
    //    string? mimeType = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.CheckAsync(
    //        containerId,
    //        fileName,
    //        length,
    //        mimeType,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task ClearContainerFilesAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.ClearContainerFilesAsync(
    //        containerId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task ClearContainerFilesAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.ClearContainerFilesAsync(
    //        container,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CopyDirectoryAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    string targetFileName = null;
    //    bool includeSubDirectory = false;
    //    Guid? targetParentId = null;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.CopyDirectoryAsync(
    //        containerId,
    //        fileId,
    //        targetFileName,
    //        includeSubDirectory,
    //        targetParentId,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CopyDirectoryAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    Guid fileId = default(global::System.Guid);
    //    string targetFileName = null;
    //    bool includeSubDirectory = false;
    //    Guid? targetParentId = null;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.CopyDirectoryAsync(
    //        container,
    //        fileId,
    //        targetFileName,
    //        includeSubDirectory,
    //        targetParentId,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CopyFileAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    string targetFileName = null;
    //    Guid? targetParentId = null;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.CopyFileAsync(
    //        containerId,
    //        fileId,
    //        targetFileName,
    //        targetParentId,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CopyFileAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    Guid fileId = default(global::System.Guid);
    //    string targetFileName = null;
    //    Guid? targetParentId = null;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.CopyFileAsync(
    //        container,
    //        fileId,
    //        targetFileName,
    //        targetParentId,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CreateAccessTokenAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    TimeSpan? expiration = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.CreateAccessTokenAsync(
    //        containerId,
    //        fileId,
    //        expiration,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CreateArchiveAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    string archiveFileName = null;
    //    bool includeSubDirectory = false;
    //    Guid? targetParentId = null;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.CreateArchiveAsync(
    //        containerId,
    //        fileId,
    //        archiveFileName,
    //        includeSubDirectory,
    //        targetParentId,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CreateArchiveAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    Guid fileId = default(global::System.Guid);
    //    string archiveFileName = null;
    //    bool includeSubDirectory = false;
    //    Guid? targetParentId = null;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.CreateArchiveAsync(
    //        container,
    //        fileId,
    //        archiveFileName,
    //        includeSubDirectory,
    //        targetParentId,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CreateDirectoryAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.CreateDirectoryAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CreateDirectoryAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.CreateDirectoryAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task DeleteAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    bool forceDelete = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.DeleteAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        forceDelete,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task DeleteAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    Guid fileId = default(global::System.Guid);
    //    bool forceDelete = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.DeleteAsync(
    //        container,
    //        fileId,
    //        forceDelete,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task DeleteAsync_StateUnderTest_ExpectedBehavior2()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    bool forceDelete = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.DeleteAsync(
    //        containerId,
    //        fileId,
    //        forceDelete,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task FindAccessTokenAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid tokenId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.FindAccessTokenAsync(
    //        tokenId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task FindAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.FindAsync(
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task FindAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.FindAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task FindAsync_StateUnderTest_ExpectedBehavior2()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.FindAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task FindDirectoryAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.FindDirectoryAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task FindDirectoryAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.FindDirectoryAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task FindFileAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.FindFileAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task FindFileAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.FindFileAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GenerateTokenAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GenerateTokenAsync(
    //        containerId,
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetAsync(
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetAsync_StateUnderTest_ExpectedBehavior2()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetDirectoryAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetDirectoryAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetDirectoryAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetDirectoryAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFileAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFileAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFileAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFileAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFileBytesAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFileBytesAsync(
    //        container,
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFileBytesAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFileBytesAsync(
    //        containerId,
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFileBytesAsync_StateUnderTest_ExpectedBehavior2()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFileBytesAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFileCountAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFileCountAsync(
    //        container,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFileSteamAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFileSteamAsync(
    //        container,
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFileSteamAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFileSteamAsync(
    //        containerId,
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFileSteamAsync_StateUnderTest_ExpectedBehavior2()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFileSteamAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetTagsAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetTagsAsync(
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetTagsAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetTagsAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetTagsAsync_StateUnderTest_ExpectedBehavior2()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetTagsAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetTagsAsync_StateUnderTest_ExpectedBehavior3()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetTagsAsync(
    //        containerId,
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsDirectoryExistsAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsDirectoryExistsAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsDirectoryExistsAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsDirectoryExistsAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsExistsAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsExistsAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsFileExistsAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsFileExistsAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsFileExistsAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsFileExistsAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsReadOnlyAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsReadOnlyAsync(
    //        container,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsReadOnlyAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid fileId = default(global::System.Guid);
    //    bool isReadOnly = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsReadOnlyAsync(
    //        fileId,
    //        isReadOnly,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsReadOnlyAsync_StateUnderTest_ExpectedBehavior2()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Guid? parentId = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsReadOnlyAsync(
    //        containerId,
    //        fileName,
    //        parentId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsReadOnlyAsync_StateUnderTest_ExpectedBehavior3()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsReadOnlyAsync(
    //        containerId,
    //        fileId,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsValidAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    long length = 0;
    //    string? mimeType = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsValidAsync(
    //        container,
    //        fileName,
    //        length,
    //        mimeType,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task IsValidAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    long length = 0;
    //    string? mimeType = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.IsValidAsync(
    //        containerId,
    //        fileName,
    //        length,
    //        mimeType,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task SaveAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    Stream stream = null;
    //    string? mimeType = null;
    //    Guid? parentId = null;
    //    bool ignoreCheck = false;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.SaveAsync(
    //        container,
    //        fileName,
    //        stream,
    //        mimeType,
    //        parentId,
    //        ignoreCheck,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task SaveAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string fileName = null;
    //    byte[] bytes = null;
    //    string? mimeType = null;
    //    Guid? parentId = null;
    //    bool ignoreCheck = false;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.SaveAsync(
    //        container,
    //        fileName,
    //        bytes,
    //        mimeType,
    //        parentId,
    //        ignoreCheck,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task SaveAsync_StateUnderTest_ExpectedBehavior2()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    Stream stream = null;
    //    string? mimeType = null;
    //    Guid? parentId = null;
    //    bool ignoreCheck = false;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.SaveAsync(
    //        containerId,
    //        fileName,
    //        stream,
    //        mimeType,
    //        parentId,
    //        ignoreCheck,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task SaveAsync_StateUnderTest_ExpectedBehavior3()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string fileName = null;
    //    byte[] bytes = null;
    //    string? mimeType = null;
    //    Guid? parentId = null;
    //    bool ignoreCheck = false;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.SaveAsync(
    //        containerId,
    //        fileName,
    //        bytes,
    //        mimeType,
    //        parentId,
    //        ignoreCheck,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task SetReadOnlyAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid fileId = default(global::System.Guid);
    //    bool isReadOnly = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.SetReadOnlyAsync(
    //        fileId,
    //        isReadOnly,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task SetReadOnlyAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    bool isReadOnly = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.SetReadOnlyAsync(
    //        containerId,
    //        fileId,
    //        isReadOnly,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task SetTagsAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid fileId = default(global::System.Guid);
    //    IEnumerable tags = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.SetTagsAsync(
    //        fileId,
    //        tags,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task SetTagsAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    IEnumerable tags = null;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.SetTagsAsync(
    //        containerId,
    //        fileId,
    //        tags,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task UnarchiveAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid fileId = default(global::System.Guid);
    //    string targetFileName = null;
    //    Guid? targetParentId = null;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.UnarchiveAsync(
    //        containerId,
    //        fileId,
    //        targetFileName,
    //        targetParentId,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task UnarchiveAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    Guid fileId = default(global::System.Guid);
    //    string targetFileName = null;
    //    Guid? targetParentId = null;
    //    bool overrideExisting = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    await manager.UnarchiveAsync(
    //        container,
    //        fileId,
    //        targetFileName,
    //        targetParentId,
    //        overrideExisting,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFilesAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    string container = null;
    //    string directoryName = null;
    //    Guid? parentId = null;
    //    bool includeSubDirectory = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFilesAsync(
    //        container,
    //        directoryName,
    //        parentId,
    //        includeSubDirectory,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFilesAsync_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    string directoryName = null;
    //    Guid? parentId = null;
    //    bool includeSubDirectory = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFilesAsync(
    //        containerId,
    //        directoryName,
    //        parentId,
    //        includeSubDirectory,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetFilesAsync_StateUnderTest_ExpectedBehavior2()
    //{
    //    // Arrange
    //    var manager = new FileManager(TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid directoryId = default(global::System.Guid);
    //    bool includeSubDirectory = false;
    //    CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

    //    // Act
    //    var result = await manager.GetFilesAsync(
    //        containerId,
    //        directoryId,
    //        includeSubDirectory,
    //        cancellationToken);

    //    // Assert
    //    Assert.True(false);
    //}
}
