using System.Threading.Tasks;
using HotChocolate.Extensions.ApolloSubgraph;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HotChocolate.Extensions.ApolloFederation;

public class ExtendsDirectiveSchemaFirstInterfaceTests
{
    [Fact]
    public async Task When_extends_is_specified_on_interface()
    {
        var schema = await Test.BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                interface IProduct @extends @key(fields: ""upc"") {
                     upc: String!
                     id: String!
                }

                type Product implements IProduct @key(fields: ""upc"") {
                     upc: String!
                     id: String!
                }

                type Query
            ");
        });

        var sut = schema.GetType<InterfaceType>("IProduct");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "extends"),
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}