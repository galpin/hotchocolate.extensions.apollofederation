using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class KeyDirectiveSchemaFirstTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Product @key(fields: ""upc"") {
                     upc: String!
                }

                type Query
            ");
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_object_multiple_times()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Product @key(fields: ""upc"") @key(fields: ""id"") {
                     upc: String!
                     id: String
                }

                type Query
            ");
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")),
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}