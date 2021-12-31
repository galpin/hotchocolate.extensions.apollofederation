using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class RequiresDirectiveCodeFirstTests
{
    [Fact]
    public async Task Ctor_correctly_configures_directive()
    {
        var schema = await Test.BuildSchemaAsync(builder =>
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
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Review");

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"id\"")));
    }
}