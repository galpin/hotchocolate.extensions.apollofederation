using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ExternalDirectiveAnnotationsTests
{
    [Fact]
    public async Task When_external_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>(nameof(Product));

        Assert.Collection(
            sut.Fields["id"].Directives,
            x => Assert.Equal("external", x.Name));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_external_is_specified_on_object_extension()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenExtension>();
            builder.AddTypeExtension<ProductExtension>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>(nameof(Product));

        Assert.Collection(
            sut.Fields["id"].Directives,
            x => Assert.Equal("external", x.Name));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    public class Product
    {
        [GraphQLKey]
        public string? Upc { get; set; }

        [GraphQLExternal]
        public int? Id { get; set; }
    }

    [GraphQLName("Product")]
    public class ProductWhenExtension
    {
        public string? Upc { get; set; }

        public int? Id { get; set; }
    }

    [ExtendObjectType(typeof(ProductWhenExtension))]
    [GraphQLKey("upc")]
    [GraphQLExternal("id")]
    public class ProductExtension
    {
    }
}