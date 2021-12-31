using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ExternalDirectiveTests
{
    [Fact]
    public async Task Ctor_correctly_configures_directive()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Test").Extends().Key("id");
                x.Field("id").External().Type<IntType>();
            });
            builder.AddQueryType(x => x.Name("Query"));
        });

        var sut = schema.GetDirectiveType("external");

        Assert.IsType<ExternalDirectiveType>(sut);
        Assert.Equal("external", sut.Name);
        Assert.False(sut.IsRepeatable);
        Assert.Empty(sut.Arguments);
        Assert.Collection(
            sut.Locations,
            x => Assert.Equal(DirectiveLocation.FieldDefinition, x));
    }
}