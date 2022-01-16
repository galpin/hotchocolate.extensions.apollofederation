using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class ExternalDirectiveCodeFirstInterfaceTests
{
    [Fact]
    public async Task When_external_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var productInterfaceType = new InterfaceType(x =>
            {
                x.Name("IProduct");
                x.Field("upc").Key().Type<NonNullType<StringType>>();
                x.Field("id").External().Type<IntType>();
            });
            builder.AddType(productInterfaceType);
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Implements(productInterfaceType);
                x.Field("upc").Key().Type<NonNullType<StringType>>();
                x.Field("id").External().Type<IntType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>("IProduct");

        Assert.Collection(
            sut.Fields["id"].Directives,
            x => Assert.Equal("external", x.Name));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }
}