using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class RequiresDirectiveAnnotationsTests
{
    [Fact]
    public async Task When_requires_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Review>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>(nameof(Review));

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_requires_is_specified_on_object_extension()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ReviewWhenObjectExtension>();
            builder.AddTypeExtension<ReviewExtension>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>(nameof(Review));

        Assert.Collection(
            sut.Fields["products"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    public class Review
    {
        public Review(int id, Product product)
        {
            Id = id;
            Product = product;
        }

        [GraphQLKey]
        public int Id { get; }

        [GraphQLRequires("upc")]
        public Product Product { get; }
    }

    public class Product
    {
        public Product(string upc)
        {
            Upc = upc;
        }

        public string Upc { get; }
    }

    [GraphQLName("Review")]
    public class ReviewWhenObjectExtension
    {
        public ReviewWhenObjectExtension(int id, IReadOnlyList<Product> products)
        {
            Id = id;
            Products = products;
        }

        public int Id { get; }

        public IReadOnlyList<Product> Products { get; }
    }

    [ExtendObjectType(typeof(ReviewWhenObjectExtension))]
    [GraphQLKey("id")]
    [GraphQLRequires("products", "upc")]
    public class ReviewExtension
    {
    }
}