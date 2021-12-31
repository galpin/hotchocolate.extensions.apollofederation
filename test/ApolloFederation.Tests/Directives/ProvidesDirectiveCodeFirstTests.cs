using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ProvidesDirectiveCodeFirstTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Review").Key("id");
                x.Field("id").Type<IntType>();
                x.Field("product").Type("Product").Provides("name");
            });
            builder.AddObjectType(x =>
            {
                x.Name("Product").Key("upc");
                x.Field("upc").Type<StringType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Review");

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "provides", ("fields", "\"name\"")));
    }
}