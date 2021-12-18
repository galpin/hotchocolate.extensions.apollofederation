using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ExternalDirectiveAnnotationsTests
{
    [Fact]
    public async Task When_extends_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(x => x.AddQueryType<Query<TypeWithPropertyDirective>>());

        var sut = schema.GetType<ObjectType>(nameof(TypeWithPropertyDirective));

        Assert.Collection(
            sut.Fields["id"].Directives,
            x => Assert.Equal("external", x.Name));
    }

    public class Query<T>
    {
        public T? Get(int id)
        {
            return default;
        }
    }

    [GraphQLExtends]
    public class TypeWithPropertyDirective
    {
        [GraphQLKey]
        [GraphQLExternal]
        public int Id { get; set; }
    }
}