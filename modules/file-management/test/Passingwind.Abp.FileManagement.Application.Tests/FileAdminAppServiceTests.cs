using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement;
using Passingwind.Abp.FileManagement.Files;
using Passingwind.Abp.FileManagement.Options;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Passingwind.Abp.FileManagement;

public class FileAdminAppServiceTests : FileManagementApplicationTestBase
{
    private readonly IFileAdminAppService _service;

    public FileAdminAppServiceTests()
    {
        _service = GetRequiredService<IFileAdminAppService>();
    }

    //[Fact]
    //public async Task GetListAsync_Empty()
    //{
    //    var result = await _service.GetListAsync(Guid.Empty,new FilePagedListRequestDto());

    //    result.TotalCount.ShouldBe(0);
    //}

    //[Fact]
    //public async Task GetListAsync_Test1()
    //{
    //    var result = await _service.GetListAsync(TestContainerId, new FilePagedListRequestDto());

    //    result.TotalCount.ShouldBeGreaterThan(0);
    //}

    //[Fact]
    //public async Task GetAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid id = default(global::System.Guid);

    //    // Act
    //    var result = await service.GetAsync(
    //        containerId,
    //        id);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task DeleteAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid id = default(global::System.Guid);

    //    // Act
    //    await service.DeleteAsync(
    //        containerId,
    //        id);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetBlobAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid id = default(global::System.Guid);

    //    // Act
    //    var result = await service.GetBlobAsync(
    //        containerId,
    //        id);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GeBlobStreamAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid id = default(global::System.Guid);

    //    // Act
    //    var result = await service.GeBlobStreamAsync(
    //        containerId,
    //        id);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task GetBlobBytesAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid id = default(global::System.Guid);

    //    // Act
    //    var result = await service.GetBlobBytesAsync(
    //        containerId,
    //        id);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CreateAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    FileCreateDto input = null;

    //    // Act
    //    var result = await service.CreateAsync(
    //        containerId,
    //        input);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CreateByStreamAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    FileCreateByStreamDto input = null;

    //    // Act
    //    var result = await service.CreateByStreamAsync(
    //        containerId,
    //        input);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CreateByBytesAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    FileCreateByBytesDto input = null;

    //    // Act
    //    var result = await service.CreateByBytesAsync(
    //        containerId,
    //        input);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task MoveAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid id = default(global::System.Guid);
    //    FileMoveAdminRequestDto input = null;

    //    // Act
    //    var result = await service.MoveAsync(
    //        containerId,
    //        id,
    //        input);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task UpdateAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    Guid id = default(global::System.Guid);
    //    FileUpdateDto input = null;

    //    // Act
    //    var result = await service.UpdateAsync(
    //        containerId,
    //        id,
    //        input);

    //    // Assert
    //    Assert.True(false);
    //}

    //[Fact]
    //public async Task CreateDirectoryAsync_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var service = new FileAdminAppService(TODO, TODO, TODO, TODO, TODO, TODO);
    //    Guid containerId = default(global::System.Guid);
    //    FileDirectoryCreateDto input = null;

    //    // Act
    //    var result = await service.CreateDirectoryAsync(
    //        containerId,
    //        input);

    //    // Assert
    //    Assert.True(false);
    //}
}
