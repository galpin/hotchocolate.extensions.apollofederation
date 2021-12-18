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
        var schema = await BuildSchemaAsync(x => x.AddQueryType<Query<TypeWithClassDirective>>());

        var sut = schema.GetType<ObjectType>(nameof(TypeWithClassDirective));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, x.Name, ("fields", "\"id\"")));
    }

    [Fact]
    public async Task When_key_is_specified_on_class_multiple_times()
    {
        var schema = await BuildSchemaAsync(x => x.AddQueryType<Query<TypeWithMultipleClassDirectives>>());

        var sut = schema.GetType<ObjectType>(nameof(TypeWithMultipleClassDirectives));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")),
            x => AssertEx.Directive(x, "key", ("fields", "\"uid\"")));
    }

    [Fact]
    public async Task When_key_is_specified_on_property()
    {
        var schema = await BuildSchemaAsync(x => x.AddQueryType<Query<TypeWithPropertyDirective>>());

        var sut = schema.GetType<ObjectType>(nameof(TypeWithPropertyDirective));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
    }

    public class Query<T>
    {
        public T? Get(int id)
        {
            return default;
        }
    }

    [GraphQLKey("id")]
    public class TypeWithClassDirective
    {
        public int Id { get; set; }
    }

    [GraphQLKey("id")]
    [GraphQLKey("uid")]
    public class TypeWithMultipleClassDirectives
    {
        public int Id { get; set; }
        public int Uid { get; set; }
    }

    public class TypeWithPropertyDirective
    {
        [GraphQLKey]
        public int Id { get; set; }
    }
}