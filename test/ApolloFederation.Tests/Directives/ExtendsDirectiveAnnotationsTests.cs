using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ExtendsDirectiveAnnotationsTests
{
    [Fact]
    public async Task When_extends_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>(nameof(Product));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "extends"),
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_extends_is_specified_on_object_extension()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenObjectExtension>();
            builder.AddTypeExtension<ProductExtension>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>(nameof(Product));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "extends"),
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [GraphQLExtends]
    public class Product
    {
        [GraphQLKey]
        public string? Upc { get; set; }
    }

    [GraphQLName("Product")]
    public class ProductWhenObjectExtension
    {
        public string? Upc { get; set; }
    }

    [ExtendObjectType(typeof(ProductWhenObjectExtension))]
    [GraphQLExtends]
    [GraphQLKey("upc")]
    public class ProductExtension
    {
    }
}