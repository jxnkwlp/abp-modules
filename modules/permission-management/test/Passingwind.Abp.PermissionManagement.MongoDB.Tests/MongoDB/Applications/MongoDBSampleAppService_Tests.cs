using Passingwind.Abp.PermissionManagement.MongoDB;
using Passingwind.Abp.PermissionManagement.Samples;
using Xunit;

namespace Passingwind.Abp.PermissionManagement.MongoDb.Applications;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleAppService_Tests : SampleAppService_Tests<PermissionManagementMongoDbTestModule>
{

}
