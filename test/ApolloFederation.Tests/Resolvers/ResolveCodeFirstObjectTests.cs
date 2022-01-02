using System;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class ResolveCodeFirstObjectTests : ResolveTestBase
{
    [Fact]
    public async Task Resolve_when_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Key().Type<NonNullType<StringType>>()
                    .Resolve(ctx => ctx.Parent<Product>().Upc);
                x.ResolveEntity(_ => new Product("1"));
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_typed_and_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
                x.ResolveEntity(_ => new Product("1"));
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Key().Type<NonNullType<StringType>>()
                    .Resolve(ctx => ctx.Parent<Product>().Upc);
                x.ResolveEntity(_ => Task.FromResult(new Product("1"))!);
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_typed_and_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
                x.ResolveEntity(_ => Task.FromResult(new Product("1"))!);
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_async_resolver()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Key().Type<NonNullType<StringType>>()
                    .Resolve(ctx => ctx.Parent<Product>().Upc);
                x.ResolveEntity(async _ =>
                {
                    await Task.Delay(500);
                    return new Product("1");
                });
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_typed_and_async_resolver()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Field(y => y.Upc).Key().Type<IdType>();
                x.ResolveEntity(async _ =>
                {
                    await Task.Delay(500);
                    return new Product("1");
                });
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_immediate_resolver_throws()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
                x.ResolveEntity(ResolveEntity);
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);

        Product? ResolveEntity(IEntityResolverContext _)
        {
            throw new InvalidOperationException();
        }
    }

    [Fact]
    public async Task Resolve_when_async_resolver_throws()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Field(y => y.Upc).Key().Type<IdType>();
                x.ResolveEntity(ResolveEntityAsync);
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);

        async Task<Product?> ResolveEntityAsync(IEntityResolverContext _)
        {
            await Task.Delay(500);
            throw new InvalidOperationException();
        }
    }

    [Fact]
    public async Task Resolve_when_immediate_resolver_returns_null()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Key().Type<NonNullType<StringType>>()
                    .Resolve(ctx => ctx.Parent<Product>().Upc);
                x.ResolveEntity(_ => (Product?)null);
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_typed_and_immediate_resolver_returns_null()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Product>(x =>
            {
                x.Field(y => y.Upc).Key().Type<NonNullType<StringType>>();
                x.ResolveEntity(_ => (Product?)null);
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver_returns_null()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Key().Type<NonNullType<StringType>>()
                    .Resolve(ctx => ctx.Parent<Product>().Upc);
                x.ResolveEntity(_ => Task.FromResult<Product?>(null));
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver_returns_null_task()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Key().Type<NonNullType<StringType>>()
                    .Resolve(ctx => ctx.Parent<Product>().Upc);
                x.ResolveEntity(ResolveEntityAsync);
            });
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);

        Task<Product?> ResolveEntityAsync(IEntityResolverContext _)
        {
            return null!;
        }
    }

    private sealed record Product(string Upc);
}