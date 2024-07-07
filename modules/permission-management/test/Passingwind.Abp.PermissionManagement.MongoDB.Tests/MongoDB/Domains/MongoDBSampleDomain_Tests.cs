using Passingwind.Abp.PermissionManagement.Samples;
using Xunit;

namespace Passingwind.Abp.PermissionManagement.MongoDB.Domains;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleDomain_Tests : SampleManager_Tests<PermissionManagementMongoDbTestModule>
{

}
