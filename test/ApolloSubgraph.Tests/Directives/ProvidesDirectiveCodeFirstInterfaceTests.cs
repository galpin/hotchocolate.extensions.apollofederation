using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class ProvidesDirectiveCodeFirstInterfaceTests
{
    [Fact]
    public async Task When_provides_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var reviewInterfaceType = new InterfaceType(x =>
            {
                x.Name("IReview");
                x.Extends();
                x.Field("id").Key().Type<NonNullType<IntType>>();
                x.Field("products").Type("[Product!]!").Provides("name");
            });
            builder.AddObjectType(x =>
            {
                x.Name("Review");
                x.Implements(reviewInterfaceType);
                x.Field("id").Key().Type<NonNullType<IntType>>();
                x.Field("products").Type("[Product!]!").Provides("name");
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
            sut.Fields["products"].Directives,
            x => AssertEx.Directive(x, "provides", ("fields", "\"name\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}