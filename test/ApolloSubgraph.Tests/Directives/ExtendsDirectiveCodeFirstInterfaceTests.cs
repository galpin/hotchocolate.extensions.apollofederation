using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class ExtendsDirectiveCodeFirstInterfaceTests
{
    [Fact]
    public async Task When_extends_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var productInterfaceType = new InterfaceType(x =>
            {
                x.Name(nameof(IProduct));
                x.Extends();
                x.Field("upc").Key().Type<NonNullType<StringType>>();
            });
            builder.AddType(productInterfaceType);
            builder.AddObjectType<Product>(x =>
            {
                x.Implements(productInterfaceType);
                x.Extends();
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "extends"),
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_typed_and_extends_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var productInterfaceType = new InterfaceType<IProduct>(x =>
            {
                x.Extends();
                x.Field("upc").Key().Type<NonNullType<StringType>>();
            });
            builder.AddObjectType<Product>(x =>
            {
                x.Implements(productInterfaceType);
                x.Extends();
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "extends"),
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    private interface IProduct
    {
        string? Upc { get; }

        string? Id { get; }

        string? GetUpc();
    }

    private sealed record Product(string Upc = "1", string Id = "id") : IProduct
    {
        public string? GetUpc()
        {
            return Upc;
        }
    }
}