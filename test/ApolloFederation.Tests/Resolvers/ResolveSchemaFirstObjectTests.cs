using System;
using System.Threading.Tasks;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class ResolveSchemaFirstObjectTests : ResolveTestBase
{
    [Fact]
    public async Task Resolve_and_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(_ => new Product("1"));
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_and_immediate_resolver_and_name_is_specified()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(nameof(Product), _ => new Product("1"));
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_and_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(_ => Task.FromResult(new Product("1"))!);
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_and_task_resolver_and_name_is_specified()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(nameof(Product), _ => Task.FromResult(new Product("1"))!);
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_and_async_resolver()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(async _ =>
            {
                await Task.Delay(500);
                return new Product("1");
            });
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_and_immediate_resolver_throws()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(ResolveEntity);
        });

        await QueryProductAndMatchSnapshotAsync(schema);

        Product ResolveEntity(IEntityResolverContext _)
        {
            throw new InvalidOperationException();
        }
    }

    [Fact]
    public async Task Resolve_and_resolver_throws()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(ResolveEntityAsync);
        });

        await QueryProductAndMatchSnapshotAsync(schema);

        async Task<Product?> ResolveEntityAsync(IEntityResolverContext _)
        {
            await Task.Delay(500);
            throw new InvalidOperationException();
        }
    }

    private static void AddSchema(IRequestExecutorBuilder builder)
    {
        builder.AddDocumentFromString(@"
            type Product @key(fields: ""upc"") @key(fields: ""id"") {
                 upc: String!
                 id: String!
            }

            type Query
        ");
        builder.BindRuntimeType<Product>();
    }

    public sealed record Product(string Upc, string Id = "id");
}