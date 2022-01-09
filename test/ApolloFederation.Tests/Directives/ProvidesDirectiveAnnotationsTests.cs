using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ProvidesDirectiveAnnotationsTests
{
    [Fact]
    public async Task When_provides_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Review>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>(nameof(Review));

        Assert.Collection(
            sut.Fields["products"].Directives,
            x => AssertEx.Directive(x, "provides", ("fields", "\"name\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_provides_is_specified_on_object_extension()
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
            x => AssertEx.Directive(x, "provides", ("fields", "\"name\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    public class Review
    {
        public Review(int id, IReadOnlyList<Product> products)
        {
            Id = id;
            Products = products;
        }

        [GraphQLKey]
        [GraphQLExternal]
        public int Id { get; }

        [GraphQLProvides("name")]
        public IReadOnlyList<Product> Products { get; }
    }

    public class Product
    {
        public Product(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        [GraphQLExternal]
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
    [GraphQLProvides("products", "name")]
    public class ReviewExtension
    {
    }
}