using Passingwind.Abp.DynamicPermissionManagement.Samples;
using Xunit;

namespace Passingwind.Abp.DynamicPermissionManagement.MongoDB.Samples;

[Collection(MongoTestCollection.Name)]
public class SampleRepository_Tests : SampleRepository_Tests<DynamicPermissionManagementMongoDbTestModule>
{
    /* Don't write custom repository tests here, instead write to
     * the base class.
     * One exception can be some specific tests related to MongoDB.
     */
}
