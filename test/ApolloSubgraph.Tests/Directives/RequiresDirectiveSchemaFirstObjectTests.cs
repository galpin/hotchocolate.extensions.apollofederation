using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class RequiresDirectiveSchemaFirstObjectTests
{
    [Fact]
    public async Task When_requires_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Review @key(fields: ""id"") {
                     id: Int
                     product: Product @requires(fields: ""id"")
                }

                type Product @key(fields: ""upc"") {
                     name: String
                }

                type Query
            ");
        });

        var sut = schema.GetType<ObjectType>("Review");

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"id\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}