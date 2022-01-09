using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class RequiresDirectiveTests
{
    [Fact]
    public async Task Ctor_correctly_configures_directive()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Review").Key("id");
                x.Field("id").Type<IntType>();
                x.Field("product").Type("Product").Requires("id");
            });
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("name").Type<StringType>();
            });
            builder.AddQueryType(x => x.Name("Query"));
        });

        var sut = schema.GetDirectiveType("requires");

        Assert.IsType<RequiresDirectiveType>(sut);
        Assert.Equal("requires", sut.Name);
        Assert.False(sut.IsRepeatable);
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
            x => Assert.Equal(DirectiveLocation.FieldDefinition, x));
    }
}