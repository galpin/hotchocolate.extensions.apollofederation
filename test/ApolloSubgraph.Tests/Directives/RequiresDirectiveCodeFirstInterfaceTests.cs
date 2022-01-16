using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class RequiresDirectiveCodeFirstInterfaceTests
{
    [Fact]
    public async Task When_requires_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var reviewInterfaceType = new InterfaceType(x =>
            {
                x.Name("IReview");
                x.Extends();
                x.Field("id").Key().Type<NonNullType<IntType>>();
                x.Field("product").Type("Product!").Requires("upc");
            });
            builder.AddType(reviewInterfaceType);
            builder.AddObjectType(x =>
            {
                x.Implements(reviewInterfaceType);
                x.Name("Review").Key("id");
                x.Field("id").Key().Type<NonNullType<IntType>>();
                x.Field("product").Type("Product!").Requires("upc");
            });
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Type<StringType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Review");

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}