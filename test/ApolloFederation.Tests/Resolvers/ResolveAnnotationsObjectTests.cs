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
            builder.AddObjectType<ProductWhenImmediateResolver>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenTaskResolver>();
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
            builder.AddObjectType<ProductWhenAsyncResolver>();
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
            builder.AddObjectType<ProductWhenMultipleResolvers>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_immediate_resolver_throws()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenThrowingImmediateResolver>();
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
            builder.AddObjectType<ProductWhenThrowingAsyncResolver>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_immediate_resolver_returns_null()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenImmediateResolverReturnsNull>();
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
            builder.AddObjectType<ProductWhenTaskResolverReturnsNull>();
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
            builder.AddObjectType<ProductWhenTaskResolverReturnsNullTask>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_attribute_and_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<ProductWhenAttributeAndImmediateResolver>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenImmediateResolver
    {
        public ProductWhenImmediateResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static ProductWhenImmediateResolver ResolveEntity(IEntityResolverContext _)
        {
            return new ProductWhenImmediateResolver("1");
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenTaskResolver
    {
        public ProductWhenTaskResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static Task<ProductWhenTaskResolver> ResolveEntityAsync(IEntityResolverContext _)
        {
            return Task.FromResult(new ProductWhenTaskResolver("1"));
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenAsyncResolver
    {
        public ProductWhenAsyncResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static async Task<ProductWhenAsyncResolver> ResolveEntityAsync(IEntityResolverContext _)
        {
            await Task.Delay(500);
            return new ProductWhenAsyncResolver("1");
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenMultipleResolvers
    {
        public ProductWhenMultipleResolvers(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static ProductWhenMultipleResolvers ResolveEntity(IEntityResolverContext _)
        {
            return new ProductWhenMultipleResolvers("1");
        }

        public static Task<ProductWhenMultipleResolvers> ResolveEntityAsync(IEntityResolverContext _)
        {
            throw new InvalidOperationException();
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenThrowingImmediateResolver
    {
        public ProductWhenThrowingImmediateResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static ProductWhenThrowingImmediateResolver ResolveEntity(IEntityResolverContext _)
        {
            throw new InvalidOperationException();
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenThrowingAsyncResolver
    {
        public ProductWhenThrowingAsyncResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static async Task<ProductWhenThrowingAsyncResolver> ResolveEntity(IEntityResolverContext _)
        {
            await Task.Delay(500);
            throw new InvalidOperationException();
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenImmediateResolverReturnsNull
    {
        public ProductWhenImmediateResolverReturnsNull(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static ProductWhenImmediateResolverReturnsNull? ResolveEntity(IEntityResolverContext _)
        {
            return null;
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenTaskResolverReturnsNull
    {
        public ProductWhenTaskResolverReturnsNull(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static Task<ProductWhenTaskResolverReturnsNull?> ResolveEntityAsync(IEntityResolverContext _)
        {
            return Task.FromResult<ProductWhenTaskResolverReturnsNull?>(null);
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenTaskResolverReturnsNullTask
    {
        public ProductWhenTaskResolverReturnsNullTask(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        public static Task<ProductWhenTaskResolverReturnsNullTask>? ResolveEntityAsync(IEntityResolverContext _)
        {
            return null;
        }
    }

    [GraphQLName("Product")]
    public sealed class ProductWhenAttributeAndImmediateResolver
    {
        public ProductWhenAttributeAndImmediateResolver(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }

        [EntityResolver]
        public static ProductWhenAttributeAndImmediateResolver GetEntity(IEntityResolverContext _)
        {
            return new ProductWhenAttributeAndImmediateResolver("1");
        }
    }
}