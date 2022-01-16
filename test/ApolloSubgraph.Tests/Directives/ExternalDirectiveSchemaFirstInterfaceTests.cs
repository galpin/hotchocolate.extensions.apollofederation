using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class ExternalDirectiveSchemaFirstInterfaceTests
{
    [Fact]
    public async Task When_external_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                interface IProduct @key(fields: ""upc"") {
                     upc: String!
                     id: String @external
                }

                type Product @key(fields: ""upc"") {
                     upc: String!
                     id: String @external
                }

                type Query
            ");
        });

        var sut = schema.GetType<InterfaceType>("IProduct");

        Assert.Collection(
            sut.Fields["id"].Directives,
            x => Assert.Equal("external", x.Name));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}