using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ExtendsDirectiveTests
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
            });
            builder.AddQueryType();
        });

        var sut = schema.GetDirectiveType("extends");

        Assert.IsType<ExtendsDirectiveType>(sut);
        Assert.Equal("extends", sut.Name);
        Assert.False(sut.IsRepeatable);
        Assert.Empty(sut.Arguments);
        Assert.Collection(
            sut.Locations,
            x => Assert.Equal(DirectiveLocation.Object, x),
            x => Assert.Equal(DirectiveLocation.Interface, x));
    }
}