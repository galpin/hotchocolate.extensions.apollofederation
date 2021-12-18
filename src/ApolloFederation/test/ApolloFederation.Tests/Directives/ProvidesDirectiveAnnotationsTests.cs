using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

#nullable disable

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ProvidesDirectiveAnnotationsTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(x => x.AddQueryType<Query<Review>>());

        var sut = schema.GetType<ObjectType>(nameof(Review));

        Assert.Collection(
            sut.Fields["products"].Directives,
            x => AssertEx.Directive(x, "provides", ("fields", "\"name\"")));
    }

    public class Query<T>
    {
        public T Get(int id)
        {
            return default;
        }
    }

    public class Review
    {
        [GraphQLKey]
        [GraphQLExternal]
        public int Id { get; set; }

        [GraphQLProvides("name")]
        public IReadOnlyList<Product> Products { get; set; }
    }

    public class Product
    {
        [GraphQLKey]
        [GraphQLExternal]
        public string Upc { get; set; }
    }
}