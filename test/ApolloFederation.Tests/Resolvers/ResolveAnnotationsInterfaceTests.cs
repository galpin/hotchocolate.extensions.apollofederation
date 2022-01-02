using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class ResolveAnnotationsInterfaceTests : ResolveTestBase
{
    [Fact]
    public async Task Resolve_when_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddInterfaceType<IProduct>();
            builder.AddObjectType<Product>();
            builder.AddQueryType();
            builder.AddEntityResolver<IProduct>(_ => new Product("1"));
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_task_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddInterfaceType<IProduct>();
            builder.AddObjectType<Product>();
            builder.AddQueryType();
            builder.AddEntityResolver(_ => Task.FromResult<IProduct?>(new Product("1")));
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);
    }

    [Fact]
    public async Task Resolve_when_async_resolver()
    {
        Snapshot.FullName();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddInterfaceType<IProduct>();
            builder.AddObjectType<Product>();
            builder.AddQueryType();
            builder.AddEntityResolver<IProduct>(async _ =>
            {
                await Task.Delay(500);
                return new Product("1");
            });
        });

        await QueryProductInterfaceAndMatchSnapshotAsync(schema);
    }

    public interface IProduct
    {
        [GraphQLKey]
        string? Upc { get; }
    }

    public sealed class Product : IProduct
    {
        public Product(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }
    }
}