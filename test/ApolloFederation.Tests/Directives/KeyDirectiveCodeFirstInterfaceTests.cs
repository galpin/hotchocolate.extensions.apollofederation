using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class KeyDirectiveCodeFirstInterfaceTests
{
    [Fact]
    public async Task When_key_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductWhenKeyIsSpecifiedOnInterfaceType>();
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
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductWhenKeyIsSpecifiedOnFieldType>();
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
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductWhenStringKeyIsSpecifiedOnInterface>();
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
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductWhenKeyIsSpecifiedOnInterfaceWhenPropertyExpressionType>();
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
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductWhenKeyIsSpecifiedOnInterfaceWhenMethodExpressionType>();
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
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductWhenKeyIsSpecifiedOnInterfaceMultipleTimes>();
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

    private sealed class IProductWhenKeyIsSpecifiedOnInterfaceType : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key("upc");
            descriptor.Field("upc").Type<NonNullType<StringType>>();
        }
    }

    private sealed class IProductWhenKeyIsSpecifiedOnFieldType : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Field("upc").Key().Type<NonNullType<StringType>>();
        }
    }

    private sealed class IProductWhenStringKeyIsSpecifiedOnInterface : InterfaceType<IProduct>
    {
        protected override void Configure(IInterfaceTypeDescriptor<IProduct> descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key("upc");
            descriptor.Field(x => x.Upc).Type<NonNullType<StringType>>();
        }
    }

    private sealed class IProductWhenKeyIsSpecifiedOnInterfaceWhenPropertyExpressionType : InterfaceType<IProduct>
    {
        protected override void Configure(IInterfaceTypeDescriptor<IProduct> descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key(x => x.Upc);
            descriptor.Field(x => x.Upc).Type<NonNullType<StringType>>();
        }
    }

    private sealed class IProductWhenKeyIsSpecifiedOnInterfaceWhenMethodExpressionType : InterfaceType<IProduct>
    {
        protected override void Configure(IInterfaceTypeDescriptor<IProduct> descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key(x => x.GetUpc());
            descriptor.Field(x => x.GetUpc()).Type<NonNullType<StringType>>();
        }
    }

    private sealed class IProductWhenKeyIsSpecifiedOnInterfaceMultipleTimes : InterfaceType<IProduct>
    {
        protected override void Configure(IInterfaceTypeDescriptor<IProduct> descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key(x => x.GetUpc()).Key(x => x.Id);
            descriptor.Field(x => x.Upc).Type<NonNullType<StringType>>();
        }
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