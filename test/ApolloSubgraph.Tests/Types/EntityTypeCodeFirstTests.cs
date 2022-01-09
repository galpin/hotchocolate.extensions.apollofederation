using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Types;

public class EntityTypeCodeFirstTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product").Key("id");
                x.Field("id").Type<IntType>();
            });
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
            builder.AddObjectType(x =>
            {
                x.Name("Product").Key("id");
                x.Field("id").Type<IntType>();
            });
            builder.AddObjectType(x =>
            {
                x.Name("Review");
                x.Field("id").Key().Type<IntType>();
            });
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
            builder.AddType<ProductType>();
            builder.AddType<ProductTypeExtension>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name));
    }

    private sealed class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Field(x => x.Upc).Type<NonNullType<StringType>>();
        }
    }

    private sealed class ProductTypeExtension : ObjectTypeExtension<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Extends();
            descriptor.Key(x => x.Upc);
            descriptor.ResolveEntity(_ => new Product("1"));
        }
    }

    private sealed record Product(string Upc);
}