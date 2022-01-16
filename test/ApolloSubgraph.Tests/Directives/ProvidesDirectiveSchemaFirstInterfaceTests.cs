using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class ProvidesDirectiveSchemaFirstInterfaceTests
{
    [Fact]
    public async Task When_extends_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                interface IReview @key(fields: ""id"") {
                     id: Int
                     products: [Product!]! @provides(fields: ""name"")
                }

                type Review implements IReview @key(fields: ""id"") {
                     id: Int
                     products: [Product!]! @provides(fields: ""name"")
                }

                type Product @key(fields: ""upc"") {
                     upc: String
                }

                type Query
            ");
        });

        var sut = schema.GetType<InterfaceType>("IReview");

        Assert.Collection(
            sut.Fields["products"].Directives,
            x => AssertEx.Directive(x, "provides", ("fields", "\"name\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}