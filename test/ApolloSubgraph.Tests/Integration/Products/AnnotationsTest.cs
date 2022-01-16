using System.Collections.Generic;
using System.Linq;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloSubgraph.Integration.Products;

public class AnnotationsTest : ProductsTestBase
{
    protected override IRequestExecutorBuilder CreateRequestExecutorBuilder()
    {
        return new ServiceCollection()
            .AddSingleton<ProductsRepository>()
            .AddGraphQL()
            .AddApolloSubgraph()
            .AddType<Product>()
            .AddType<ProductDimension>()
            .AddType<ProductVariation>()
            .AddType<User>()
            .AddQueryType<Query>();
    }

    [GraphQLKey("id")]
    [GraphQLKey("sku package")]
    [GraphQLKey("sku variation { id }")]
    public sealed class Product
    {
        public Product(
            string id,
            string sku,
            string package,
            ProductVariation variation,
            ProductDimension dimensions,
            User createdBy)
        {
            Id = id;
            Sku = sku;
            Package = package;
            Variation = variation;
            Dimensions = dimensions;
            CreatedBy = createdBy;
        }

        [GraphQLType(typeof(IdType))]
        public string Id { get; }

        public string? Sku { get; }

        public string? Package { get; }

        public ProductVariation? Variation { get; }

        public ProductDimension? Dimensions { get; }

        [GraphQLProvides("totalProductsCreated")]
        public User? CreatedBy { get; }

        public static Product? ResolveEntity(IEntityResolverContext ctx)
        {
            var products = ctx.Service<ProductsRepository>();

            var id = ctx.Representation.GetValueOrDefault<string>("id");
            if (id != null)
            {
                return products.Find(id);
            }

            var sku = ctx.Representation.GetValueOrDefault<string>("sku");
            var package = ctx.Representation.GetValueOrDefault<string>("package");
            if (sku != null && package != null)
            {
                return products.Find(sku: sku, package: package);
            }

            var variation = ctx.Representation
                .GetValueOrDefault<IReadOnlyDictionary<string, object?>>("variation")?
                .GetValueOrDefault<string>("id");
            if (sku != null && variation != null)
            {
                return products.Find(sku: sku, variation: variation);
            }

            return null;
        }
    }

    public sealed class ProductDimension
    {
        public ProductDimension(string size, double weight)
        {
            Size = size;
            Weight = weight;
        }

        public string? Size { get; }

        public double? Weight { get; }
    }

    public sealed class ProductVariation
    {
        public ProductVariation(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    [GraphQLExtends]
    public sealed class User
    {
        public User(string email, int totalProductsCreated)
        {
            Email = email;
            TotalProductsCreated = totalProductsCreated;
        }

        [GraphQLKey]
        [GraphQLExternal]
        public string Email { get; }

        [GraphQLExternal]
        public int? TotalProductsCreated { get; }
    }

    public sealed class ProductsRepository
    {
        private static readonly Product[] s_products =
        {
            new(
                id: "apollo-federation",
                sku: "federation",
                package: "@apollo/federation",
                variation: new ProductVariation("OSS"),
                dimensions: new ProductDimension(size: "1", weight: 1),
                createdBy: new User("support@apollographql.com", totalProductsCreated: 1991)),
            new(
                id: "apollo-studio",
                sku: "studio",
                package: string.Empty,
                variation: new ProductVariation("platform"),
                dimensions: new ProductDimension(size: "1", weight: 1),
                createdBy: new User("support@apollographql.com", totalProductsCreated: 1991)),
        };

        public Product? Find(
            string? id = null,
            string? sku = null,
            string? package = null,
            string? variation = null)
        {
            var query = s_products.AsEnumerable();
            if (id != null)
            {
                query = query.Where(x => x.Id == id);
            }
            if (sku != null)
            {
                query = query.Where(x => x.Sku == sku);
            }
            if (package != null)
            {
                query = query.Where(x => x.Package == package);
            }
            if (variation != null)
            {
                query = query.Where(x => x.Variation?.Id == variation);
            }
            return query.SingleOrDefault();
        }
    }

    public sealed class Query
    {
        public Product? GetProduct([Service] ProductsRepository products, string id)
        {
            return products.Find(id);
        }
    }
}