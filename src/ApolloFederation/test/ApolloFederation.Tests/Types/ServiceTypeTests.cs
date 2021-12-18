using System.Threading.Tasks;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Types;

public class ServiceTypeTests
{
    [Fact]
    public async Task Ctor_correctly_configures_object()
    {
        var schema = await BuildSchemaAsync();

        var sut = schema.GetType<ServiceType>("_Service");

        Assert.Equal("_Service", sut.Name);
    }
}