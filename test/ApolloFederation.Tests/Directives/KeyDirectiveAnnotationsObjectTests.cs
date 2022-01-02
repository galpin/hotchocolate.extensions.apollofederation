// ReSharper disable MemberCanBePrivate.Global

using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class KeyDirectiveAnnotationsObjectTests
{
    [Fact]
    public async Task When_key_is_specified_on_class()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenKeyIsSpecifiedOnClass>(x => x.Name("Product"));
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, x.Name, ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_class_multiple_times()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenKeyIsSpecifiedOnClassMultipleTimes>();
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
    public async Task When_key_is_specified_on_property()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenKeyIsSpecifiedOnProperty>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [GraphQLKey("upc")]
    [GraphQLName("Product")]
    public class ProductWhenKeyIsSpecifiedOnClass
    {
        public string? Upc { get; set; }
    }

    [GraphQLKey("upc")]
    [GraphQLKey("id")]
    [GraphQLName("Product")]
    public class ProductWhenKeyIsSpecifiedOnClassMultipleTimes
    {
        public string? Upc { get; set; }
        public string? Id { get; set; }
    }

    [GraphQLName("Product")]
    public class ProductWhenKeyIsSpecifiedOnProperty
    {
        [GraphQLKey]
        public string? Upc { get; set; }
    }
}