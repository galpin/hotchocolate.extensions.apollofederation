using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class KeyDirectiveTests
{
    [Fact]
    public async Task Ctor_correctly_configures_directive()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product").Extends().Key("upc");
                x.Field("upc").Type<NonNullType<StringType>>();
                x.Field("id").External().Type<StringType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetDirectiveType("key");

        Assert.IsType<KeyDirectiveType>(sut);
        Assert.Equal("key", sut.Name);
        Assert.True(sut.IsRepeatable);
        Assert.Collection(
            sut.Arguments,
            x =>
            {
                Assert.Equal("fields", x.Name);
                var notNull = Assert.IsType<NonNullType>(x.Type);
                Assert.IsType<FieldSetType>(notNull.Type);
            }
        );
        Assert.Collection(
            sut.Locations,
            x => Assert.Equal(DirectiveLocation.Object, x),
            x => Assert.Equal(DirectiveLocation.Interface, x));
    }
}