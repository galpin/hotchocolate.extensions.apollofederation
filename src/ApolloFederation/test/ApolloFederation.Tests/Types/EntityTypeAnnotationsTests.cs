using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Types;

public class EntityTypeAnnotationsTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(x => x.AddQueryType<QueryWithSingle>());

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name));
    }

    [Fact]
    public async Task When_key_is_specified_on_multiple_objects()
    {
        var schema = await BuildSchemaAsync(x => x.AddQueryType<QueryWithMultiple>());

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name),
            x => Assert.Equal("Review", x.Name));
    }

    public class QueryWithSingle
    {
        public Product? GetProduct(int id)
        {
            return default;
        }
    }

    public class QueryWithMultiple
    {
        public Product? GetProduct(int id)
        {
            return default;
        }

        public Review? GetReview(int id)
        {
            return default;
        }
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
}