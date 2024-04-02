using System.Threading.Tasks;
using Passingwind.Abp.DictionaryManagement.Dictionaries;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace Passingwind.Abp.DictionaryManagement;

public class DictionaryManagerTests : DictionaryManagementDomainTestBase
{
    private readonly DictionaryManager _dictionaryManager;

    public DictionaryManagerTests()
    {
        _dictionaryManager = GetRequiredService<DictionaryManager>();
    }

    [Theory]
    [InlineData("abc", true)]
    [InlineData("123", true)]
    [InlineData("ab12", true)]
    [InlineData("a_1", true)]
    [InlineData("a-1", true)]
    [InlineData("a.1", true)]
    [InlineData("a 1", false)]
    [InlineData("a!1", false)]
    [InlineData("a@1", false)]
    [InlineData("a=1", false)]
    [InlineData("a+1", false)]
    [InlineData(" ", false)]
    public void CheckNameValid(string name, bool result)
    {
        DictionaryManager.IsNameValid(name).ShouldBe(result);
    }

    [Theory]
    [InlineData("a", true)]
    [InlineData("1", true)]
    public async Task GroupTest(string name, bool result)
    {
        _ = await _dictionaryManager.CreateGroupAsync(name, name);

        (await _dictionaryManager.IsGroupExistsAsync(name)).ShouldBe(result);
        (await _dictionaryManager.IsGroupExistsAsync(name + "abc")).ShouldBe(!result);

        await Should.ThrowAsync<BusinessException>(async () => await _dictionaryManager.CreateGroupAsync(name, name));

        await Should.NotThrowAsync(async () => await _dictionaryManager.GetGroupAsync(name));

        // delete
        await _dictionaryManager.DeleteGroupAsync(name);
        (await _dictionaryManager.IsGroupExistsAsync(name)).ShouldBe(false);
    }

    [Theory]
    [InlineData("a1", "a2", null, false)]
    [InlineData("a1", "a2", "", false)]
    [InlineData("a2", "a3", null, true)]
    public async Task GroupCreateTest(string name, string displayName, string parentName, bool isPublic)
    {
        var item = await _dictionaryManager.CreateGroupAsync(name, displayName, parentName, isPublic: isPublic);

        item.Name.ShouldBe(name);
        item.DisplayName.ShouldBe(displayName);
        item.ParentName.ShouldBe(parentName);
        item.IsPublic.ShouldBe(isPublic);
    }

    [Theory]
    [InlineData("a1", "a2", null, false)]
    [InlineData("a2", "a3", null, true)]
    public async Task GroupPublicTest(string name, string displayName, string parentName, bool isPublic)
    {
        await _dictionaryManager.CreateGroupAsync(name, displayName, parentName, isPublic: isPublic);

        await _dictionaryManager.SetGroupPublicAsync(name, !isPublic);

        var item = await _dictionaryManager.GetGroupAsync(name);

        item.Name.ShouldBe(name);
        item.DisplayName.ShouldBe(displayName);
        item.ParentName.ShouldBe(parentName);
        item.IsPublic.ShouldBe(!isPublic);
    }

    [Theory]
    [InlineData("a", true)]
    [InlineData("1", true)]
    public async Task ItemTest(string name, bool result)
    {
        _ = await _dictionaryManager.CreateItemAsync(name, name);

        (await _dictionaryManager.IsItemExistsAsync(name)).ShouldBe(result);
        (await _dictionaryManager.IsItemExistsAsync(name + "abc")).ShouldBe(!result);

        await Should.ThrowAsync<BusinessException>(async () => await _dictionaryManager.CreateItemAsync(name, name));

        await Should.NotThrowAsync(async () => await _dictionaryManager.GetItemAsync(name));

        // delete
        await _dictionaryManager.DeleteItemAsync(name);
        (await _dictionaryManager.IsItemExistsAsync(name)).ShouldBe(false);
    }

    [Theory]
    [InlineData("a1", "a2", null, false)]
    [InlineData("a1", "a2", "", false)]
    [InlineData("a2", "a3", null, true)]
    public async Task ItemCreateTest(string name, string displayName, string parentName, bool isEnabled)
    {
        var item = await _dictionaryManager.CreateItemAsync(name, displayName, parentName, isEnabled: isEnabled);

        item.Name.ShouldBe(name);
        item.DisplayName.ShouldBe(displayName);
        item.IsEnabled.ShouldBe(isEnabled);
    }

    [Theory]
    [InlineData("a1", "a2", null, false)]
    [InlineData("a2", "a3", null, true)]
    public async Task ItemEnabledTest(string name, string displayName, string parentName, bool isEnabled)
    {
        await _dictionaryManager.CreateItemAsync(name, displayName, parentName, isEnabled: isEnabled);

        await _dictionaryManager.SetItemEnabledAsync(name, !isEnabled);

        var item = await _dictionaryManager.GetItemAsync(name);

        item.Name.ShouldBe(name);
        item.DisplayName.ShouldBe(displayName);
        item.IsEnabled.ShouldBe(!isEnabled);
    }

    [Theory]
    [InlineData("a1", "a2")]
    [InlineData("a2", "a2")]
    [InlineData("a3", "")]
    [InlineData("a4", null)]
    public async Task ItemValueTest(string name, string value)
    {
        await _dictionaryManager.CreateItemAsync(name, name);

        await _dictionaryManager.SetItemValueAsync(name, value);

        var item = await _dictionaryManager.GetItemAsync(name);

        item.Name.ShouldBe(name);
        item.Value.ShouldBe(value);

        var value2 = await _dictionaryManager.GetItemValueAsync(name);

        value2.ShouldBe(value);
    }
}
