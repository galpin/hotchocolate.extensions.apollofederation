using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ExternalDirectiveCodeFirstTests
{
    [Fact]
    public async Task When_extends_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Test").Extends().Key("id");
                x.Field("id").External().Type<IntType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Test");

        Assert.Collection(
            sut.Fields["id"].Directives,
            x => Assert.Equal("external", x.Name));
    }
}