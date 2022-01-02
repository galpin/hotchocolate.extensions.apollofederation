using System.Threading.Tasks;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class ResolveSchemaFirstInterfaceTests : ResolveTestBase
{
    [Fact]
    public async Task Resolve_when_interface_and_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(ResolveEntityAsync);
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);

        IProduct? ResolveEntityAsync(IEntityResolverContext _)
        {
            return new Product("1");
        }
    }

    [Fact]
    public async Task Resolve_when_interface_and_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(ResolveEntityAsync);
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);

        Task<IProduct?> ResolveEntityAsync(IEntityResolverContext _)
        {
            return Task.FromResult<IProduct?>(new Product("1"));
        }
    }

    [Fact]
    public async Task Resolve_when_interface_and_async_resolver()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            AddSchema(builder);
            builder.AddEntityResolver(ResolveEntityAsync);
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);

        async Task<IProduct?> ResolveEntityAsync(IEntityResolverContext _)
        {
            await Task.Delay(500);
            return new Product("1");
        }
    }

    private static void AddSchema(IRequestExecutorBuilder builder)
    {
        builder.AddDocumentFromString(@"
            interface IProduct @key(fields: ""upc"") @key(fields: ""id"") {
                 upc: String!
                 id: String!
            }

            type Product implements IProduct @key(fields: ""upc"") @key(fields: ""id"") {
                 upc: String!
                 id: String!
            }

            type Query
        ");
        builder.BindRuntimeType<Product>();
    }

    private interface IProduct
    {
        string Upc { get; }
    }

    private sealed record Product(string Upc) : IProduct;
}