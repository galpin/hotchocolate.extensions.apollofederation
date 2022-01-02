using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class ResolveCodeFirstInterfaceTests : ResolveTestBase
{
    [Fact]
    public async Task Resolve_when_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductTypeWhenImmediateResolver>();
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_typed_and_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductTypeWhenTypedAndImmediateResolver>();
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductTypeWhenTaskResolver>();
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_typed_and_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductTypeWhenTypedAndTaskResolver>();
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_async_resolver()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductTypeWhenAsyncResolver>();
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_typed_and_async_resolver()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Implements<IProductTypeWhenTypedAndAsyncResolver>();
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
            });
            builder.AddQueryType();
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);
    }

    private sealed class IProductTypeWhenImmediateResolver : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key("upc");
            descriptor.Field("upc").Type<NonNullType<StringType>>();
            descriptor.ResolveEntity(_ => new Product("1"));
        }
    }

    private sealed class IProductTypeWhenTypedAndImmediateResolver : InterfaceType<IProduct>
    {
        protected override void Configure(IInterfaceTypeDescriptor<IProduct> descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key(x => x.Upc);
            descriptor.Field(x => x.Upc).Type<NonNullType<StringType>>();
            descriptor.ResolveEntity(_ => new Product("1"));
        }
    }

    private sealed class IProductTypeWhenTaskResolver : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key("upc");
            descriptor.Field("upc").Type<NonNullType<StringType>>();
            descriptor.ResolveEntity(_ => Task.FromResult(new Product("1"))!);
        }
    }

    private sealed class IProductTypeWhenTypedAndTaskResolver : InterfaceType<IProduct>
    {
        protected override void Configure(IInterfaceTypeDescriptor<IProduct> descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key(x => x.Upc);
            descriptor.Field(x => x.Upc).Type<NonNullType<StringType>>();
            descriptor.ResolveEntity(_ => Task.FromResult<IProduct>(new Product("1"))!);
        }
    }

    private sealed class IProductTypeWhenAsyncResolver : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key("upc");
            descriptor.Field("upc").Type<NonNullType<StringType>>();
            descriptor.ResolveEntity(async _ =>
            {
                await Task.Delay(500);
                return new Product("1");
            });
        }
    }

    private sealed class IProductTypeWhenTypedAndAsyncResolver : InterfaceType<IProduct>
    {
        protected override void Configure(IInterfaceTypeDescriptor<IProduct> descriptor)
        {
            descriptor.Name(nameof(IProduct));
            descriptor.Key(x => x.Upc);
            descriptor.Field(x => x.Upc).Type<NonNullType<StringType>>();
            descriptor.ResolveEntity<IProduct>(async _ =>
            {
                await Task.Delay(500);
                return new Product("1");
            });
        }
    }

    private interface IProduct
    {
        string? Upc { get; }
    }

    private sealed record Product(string Upc) : IProduct;
}