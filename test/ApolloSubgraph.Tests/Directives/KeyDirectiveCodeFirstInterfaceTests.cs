using System.Threading.Tasks;
using HotChocolate.Extensions.ApolloSubgraph;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloFederation;

public class KeyDirectiveCodeFirstInterfaceTests
{
    [Fact]
    public async Task When_key_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var productInterfaceType = new InterfaceType(x =>
            {
                x.Name(nameof(IProduct));
                x.Key("upc");
                x.Field("upc").Type<NonNullType<StringType>>();
            });
            builder.AddObjectType<Product>(x =>
            {
                x.Implements(productInterfaceType);
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_field()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var productInterfaceType = new InterfaceType(x =>
            {
                x.Name(nameof(IProduct));
                x.Field("upc").Key().Type<NonNullType<StringType>>();
            });
            builder.AddObjectType<Product>(x =>
            {
                x.Implements(productInterfaceType);
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_string_key_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var productInterfaceType = new InterfaceType<IProduct>(x =>
            {
                x.Name(nameof(IProduct));
                x.Key("upc");
                x.Field(x => x.Upc).Type<NonNullType<StringType>>();
            });
            builder.AddObjectType<Product>(x =>
            {
                x.Implements(productInterfaceType);
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_interface_with_property_expression()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var productInterfaceType = new InterfaceType<IProduct>(x =>
            {
                x.Name(nameof(IProduct));
                x.Key(x => x.Upc);
                x.Field(x => x.Upc).Type<NonNullType<StringType>>();
            });
            builder.AddObjectType<Product>(x =>
            {
                x.Implements(productInterfaceType);
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_interface_with_method_expression()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var productInterfaceType = new InterfaceType<IProduct>(x =>
            {
                x.Name(nameof(IProduct));
                x.Key(x => x.GetUpc());
                x.Field(x => x.GetUpc()).Type<NonNullType<StringType>>();
            });
            builder.AddObjectType<Product>(x =>
            {
                x.Implements(productInterfaceType);
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_interface_multiple_times()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            var productInterfaceType = new InterfaceType<IProduct>(x =>
            {
                x.Name(nameof(IProduct));
                x.Key(x => x.GetUpc()).Key(x => x.Id);
                x.Field(x => x.Upc).Type<NonNullType<StringType>>();
            });
            builder.AddObjectType<Product>(x =>
            {
                x.Implements(productInterfaceType);
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")),
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
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