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
                type Test @key(fields: ""id"") {
                    id: Int!
                    name: String!
                }

                type Query
            ");
        });

        var sut = schema.GetType<ObjectType>("Test");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
    }

    [Fact]
    public async Task When_key_is_specified_on_object_multiple_times()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Test @key(fields: ""id"") @key(fields: ""uid"") {
                    id: Int!
                    uid: Int!
                    name: String!
                }

                type Query
            ");
        });

        var sut = schema.GetType<ObjectType>("Test");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")),
            x => AssertEx.Directive(x, "key", ("fields", "\"uid\"")));
    }
}