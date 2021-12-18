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
        var schema = await BuildSchemaAsync(x => x.AddQueryType<Query<TypeWithClassDirective>>());

        var sut = schema.GetType<ObjectType>(nameof(TypeWithClassDirective));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "extends"),
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
    }

    public class Query<T>
    {
        public T? Get(int id)
        {
            return default;
        }
    }

    [GraphQLExtends]
    public class TypeWithClassDirective
    {
        [GraphQLKey]
        public int Id { get; set; }
    }
}