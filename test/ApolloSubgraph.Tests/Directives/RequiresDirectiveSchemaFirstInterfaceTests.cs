using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class RequiresDirectiveSchemaFirstInterfaceTests
{
    [Fact]
    public async Task When_requires_is_specified_on_object()
    {
        var schema = await Test.BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                interface IReview @key(fields: ""id"") {
                     id: Int
                     product: Product @requires(fields: ""id"")
                }

                type Review implements IReview @key(fields: ""id"") {
                     id: Int
                     product: Product @requires(fields: ""id"")
                }

                type Product @key(fields: ""upc"") {
                     name: String
                }

                type Query
            ");
        });

        var sut = schema.GetType<InterfaceType>("IReview");

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"id\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}