using System;
using System.Threading.Tasks;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Resolvers;

public class ResolveSchemaFirstObjectTests : ResolveTestBase
{
    [Fact]
    public async Task Resolve_when_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema<Product>(builder);
            builder.AddEntityResolver(_ => new Product("1"));
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_immediate_resolver_and_name_is_specified()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema<Product>(builder);
            builder.AddEntityResolver(nameof(Product), _ => new Product("1"));
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema<Product>(builder);
            builder.AddEntityResolver(_ => Task.FromResult(new Product("1"))!);
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver_and_name_is_specified()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema<Product>(builder);
            builder.AddEntityResolver(nameof(Product), _ => Task.FromResult(new Product("1"))!);
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_async_resolver()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema<Product>(builder);
            builder.AddEntityResolver(async _ =>
            {
                await Task.Delay(500);
                return new Product("1");
            });
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_immediate_resolver_throws()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema<Product>(builder);
            builder.AddEntityResolver(ResolveEntity);
        });

        await QueryProductAndMatchSnapshotAsync(schema);

        Product ResolveEntity(IEntityResolverContext _)
        {
            throw new InvalidOperationException();
        }
    }

    [Fact]
    public async Task Resolve_when_resolver_throws()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema<Product>(builder);
            builder.AddEntityResolver(ResolveEntityAsync);
        });

        await QueryProductAndMatchSnapshotAsync(schema);

        async Task<Product?> ResolveEntityAsync(IEntityResolverContext _)
        {
            await Task.Delay(500);
            throw new InvalidOperationException();
        }
    }

    [Fact]
    public async Task Resolve_when_immediate_resolver_on_bound_type()
    {
        var schema = await BuildSchemaAsync(AddSchema<ProductWhenEntityResolver>);

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    private static void AddSchema<TProduct>(IRequestExecutorBuilder builder)
    {
        builder.AddDocumentFromString(@"
            type Product @key(fields: ""upc"") @key(fields: ""id"") {
                 upc: String!
                 id: String!
            }

            type Query
        ");
        builder.BindRuntimeType<TProduct>("Product");
    }

    public sealed record Product(string Upc, string Id = "id");

    [GraphQLName("Product")]
    public sealed record ProductWhenEntityResolver(string Upc, string Id = "id")
    {
        public static ProductWhenEntityResolver? ResolveEntity(IEntityResolverContext _)
        {
            return new ProductWhenEntityResolver("1");
        }
    }
}