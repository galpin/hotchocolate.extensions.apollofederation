using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class ProvidesDirectiveSchemaFirstObjectsTests
{
    [Fact]
    public async Task When_provides_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Review @key(fields: ""id"") {
                     id: Int
                     products: [Product!]! @provides(fields: ""name"")
                }

                type Product @key(fields: ""upc"") {
                     upc: String
                }

                type Query
            ");
        });

        var sut = schema.GetType<ObjectType>("Review");

        Assert.Collection(
            sut.Fields["products"].Directives,
            x => AssertEx.Directive(x, "provides", ("fields", "\"name\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}