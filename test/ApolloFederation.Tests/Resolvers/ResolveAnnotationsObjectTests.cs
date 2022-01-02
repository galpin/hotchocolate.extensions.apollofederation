using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class ResolveAnnotationsObjectTests : ResolveTestBase
{
    [Fact]
    public async Task Resolve_when_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWithImmediateResolver>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWithTaskResolver>();
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
            builder.AddObjectType<ProductWithAsyncResolver>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_multiple_resolvers_uses_first_that_is_viable()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWithMultipleResolvers>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_immediate_resolver_throws()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWithThrowingImmediateResolver>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_async_resolver_throws()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWithThrowingAsyncResolver>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_immediate_resolver_returns_null()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWithImmediateResolverReturnsNull>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver_returns_null()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWithTaskResolverReturnsNull>();
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
            builder.AddObjectType<ProductWithTaskResolverReturnsNullTask>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [GraphQLName("Product")]
    public sealed class ProductWithImmediateResolver
    {
        public ProductWithImmediateResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static ProductWithImmediateResolver ResolveEntity(IEntityResolverContext _)
        {
            return new ProductWithImmediateResolver("1");
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWithTaskResolver
    {
        public ProductWithTaskResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static Task<ProductWithTaskResolver> ResolveEntityAsync(IEntityResolverContext _)
        {
            return Task.FromResult(new ProductWithTaskResolver("1"));
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWithAsyncResolver
    {
        public ProductWithAsyncResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static async Task<ProductWithAsyncResolver> ResolveEntityAsync(IEntityResolverContext _)
        {
            await Task.Delay(500);
            return new ProductWithAsyncResolver("1");
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWithMultipleResolvers
    {
        public ProductWithMultipleResolvers(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static ProductWithMultipleResolvers ResolveEntity(IEntityResolverContext _)
        {
            return new ProductWithMultipleResolvers("1");
        }

        public static Task<ProductWithMultipleResolvers> ResolveEntityAsync(IEntityResolverContext _)
        {
            throw new InvalidOperationException();
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWithThrowingImmediateResolver
    {
        public ProductWithThrowingImmediateResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static ProductWithThrowingImmediateResolver ResolveEntity(IEntityResolverContext _)
        {
            throw new InvalidOperationException();
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWithThrowingAsyncResolver
    {
        public ProductWithThrowingAsyncResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static async Task<ProductWithThrowingAsyncResolver> ResolveEntity(IEntityResolverContext _)
        {
            await Task.Delay(500);
            throw new InvalidOperationException();
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWithImmediateResolverReturnsNull
    {
        public ProductWithImmediateResolverReturnsNull(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static ProductWithImmediateResolverReturnsNull? ResolveEntity(IEntityResolverContext _)
        {
            return null;
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWithTaskResolverReturnsNull
    {
        public ProductWithTaskResolverReturnsNull(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static Task<ProductWithTaskResolverReturnsNull?> ResolveEntityAsync(IEntityResolverContext _)
        {
            return Task.FromResult<ProductWithTaskResolverReturnsNull?>(null);
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWithTaskResolverReturnsNullTask
    {
        public ProductWithTaskResolverReturnsNullTask(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static Task<ProductWithTaskResolverReturnsNullTask>? ResolveEntityAsync(IEntityResolverContext _)
        {
            return null;
        }
    }
}