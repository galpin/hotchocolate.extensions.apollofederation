using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Types;

public class EntityTypeTests
{
    [Fact]
    public async Task Ctor_correctly_configures_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Product @key(fields: ""id"") {
                    id: Int
                }
                type Query
            ");
        });

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Equal("_Entity", sut.Name);
    }
}