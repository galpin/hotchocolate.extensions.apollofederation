using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class KeyDirectiveCodeFirstObjectTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product").Key("upc");
                x.Field("upc").Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_string_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Key("upc");
                x.Field(y => y.Upc).Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_object_using_property_expression()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Key(y => y.Upc);
                x.Field(y => y.Upc).Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_object_using_method_expression()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Key(y => y.GetUpc());
                x.Field(y => y.Upc).Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
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
            builder.AddObjectType(x =>
            {
                x.Name("Product").Key("upc").Key("id");
                x.Field("upc").Type<NonNullType<StringType>>();
                x.Field("id").Type<IntType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")),
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_field()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    private sealed record Product(string? Upc = "1")
    {
        public string? GetUpc()
        {
            return Upc;
        }
    }
}