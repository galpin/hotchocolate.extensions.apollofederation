using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class KeyDirectiveCodeFirstTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Test").Key("id");
                x.Field("id").Type<IntType>();
                x.Field("name").Type<StringType>();
            });
            builder.AddQueryType();
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
            builder.AddObjectType(x =>
            {
                x.Name("Test").Key("id").Key("uid");
                x.Field("id").Type<IntType>();
                x.Field("uid").Type<IntType>();
                x.Field("name").Type<StringType>();
            });
            builder.AddQueryType(x =>
                x.Name("Query")
                 .Field("Get")
                 .Argument("id", a => a.Type<IntType>())
                 .Type("Test")
            );
        });

        var sut = schema.GetType<ObjectType>("Test");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")),
            x => AssertEx.Directive(x, "key", ("fields", "\"uid\"")));
    }

    [Fact]
    public async Task When_key_is_specified_on_field()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Test");
                x.Field("id").Key().Type<IntType>();
                x.Field("name").Type<StringType>();
            });
            builder.AddQueryType(x =>
                x.Name("Query")
                 .Field("Get")
                 .Argument("id", a => a.Type<IntType>())
                 .Type("Test")
            );
        });

        var sut = schema.GetType<ObjectType>("Test");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
    }
}