// ReSharper disable MemberCanBePrivate.Global

using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class KeyDirectiveAnnotationsTests
{
    [Fact]
    public async Task When_key_is_specified_on_class()
    {
        var schema = await BuildSchemaAsync(x => x.AddQueryType<Query<ProductWithClassDirective>>());

        var sut = schema.GetType<ObjectType>(nameof(ProductWithClassDirective));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, x.Name, ("fields", "\"upc\"")));
    }

    [Fact]
    public async Task When_key_is_specified_on_class_multiple_times()
    {
        var schema = await BuildSchemaAsync(x => x.AddQueryType<Query<ProductWithMultipleClassDirectives>>());

        var sut = schema.GetType<ObjectType>(nameof(ProductWithMultipleClassDirectives));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")),
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
    }

    [Fact]
    public async Task When_key_is_specified_on_property()
    {
        var schema = await BuildSchemaAsync(x => x.AddQueryType<Query<ProductWithPropertyDirective>>());

        var sut = schema.GetType<ObjectType>(nameof(ProductWithPropertyDirective));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
    }

    public class Query<T>
    {
        public T? Get(int id)
        {
            return default;
        }
    }

    [GraphQLKey("upc")]
    public class ProductWithClassDirective
    {
        public string? Upc { get; set; }
    }

    [GraphQLKey("upc")]
    [GraphQLKey("id")]
    public class ProductWithMultipleClassDirectives
    {
        public string? Upc { get; set; }
        public string? Id { get; set; }
    }

    public class ProductWithPropertyDirective
    {
        [GraphQLKey]
        public string? Upc { get; set; }
    }
}