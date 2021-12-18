using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ProvidesDirectiveSchemaFirstTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Review @key(fields: ""id"") {
                     id: Int
                     product: Product @provides(fields: ""name"")
                }

                type Product @key(fields: ""upc"") {
                     upc: String
                }

                type Query
            ");
        });

        var sut = schema.GetType<ObjectType>("Review");

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "provides", ("fields", "\"name\"")));
    }
}