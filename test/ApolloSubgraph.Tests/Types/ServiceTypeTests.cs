using System.Threading.Tasks;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Types;

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