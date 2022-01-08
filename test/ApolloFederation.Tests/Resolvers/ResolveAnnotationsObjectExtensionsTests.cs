using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class ResolveAnnotationsObjectExtensionsTests : ResolveTestBase
{
    [Fact]
    public async Task Resolve_when_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddType<Product>();
            builder.AddTypeExtension<ProductExtension>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    public sealed class Product
    {
        public Product(string? upc)
        {
            Upc = upc;
        }

        public string? Upc { get; }
    }

    [ExtendObjectType(typeof(Product))]
    [GraphQLKey("upc")]
    public class ProductExtension
    {
        public static Product ResolveEntity(IEntityResolverContext _)
        {
            return new Product("1");
        }
    }
}