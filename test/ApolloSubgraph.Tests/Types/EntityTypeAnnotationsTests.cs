using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Types;

public class EntityTypeAnnotationsTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name));
    }

    [Fact]
    public async Task When_key_is_specified_on_multiple_objects()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>();
            builder.AddObjectType<Review>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name),
            x => Assert.Equal("Review", x.Name));
    }

    [Fact]
    public async Task When_key_is_specified_on_object_extension()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenObjectExtension>();
            builder.AddTypeExtension<ProductExtension>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name));
    }

    public class Product
    {
        [GraphQLKey]
        public int Id { get; set; }
    }

    [GraphQLKey("id")]
    public class Review
    {
        public int Id { get; set; }
    }

    [GraphQLName("Product")]
    public class ProductWhenObjectExtension
    {
    }

    [ExtendObjectType(typeof(ProductWhenObjectExtension))]
    public class ProductExtension
    {
        [GraphQLKey]
        public string? Upc { get; set; }
    }
}