using System.Collections.Generic;
using System.Linq;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloSubgraph.Integration.Products;

public class CodeFirstTest : ProductsTestBase
{
    protected override IRequestExecutorBuilder CreateRequestExecutorBuilder()
    {
        return new ServiceCollection()
            .AddSingleton<ProductsRepository>()
            .AddGraphQL()
            .AddApolloSubgraph()
            .AddType<ProductType>()
            .AddType<ProductDimensionType>()
            .AddType<ProductVariationType>()
            .AddType<UserType>()
            .AddQueryType<QueryType>();
    }

    private sealed class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Key("id");
            descriptor.Key("sku package");
            descriptor.Key("sku variation { id }");
            descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
            descriptor.Field(x => x.Sku).Type<StringType>();
            descriptor.Field(x => x.Variation).Type<ProductVariationType>();
            descriptor.Field(x => x.Dimensions).Type<ProductDimensionType>();
            descriptor.Field(x => x.CreatedBy).Type<UserType>().Provides("totalProductsCreated");
            descriptor.ResolveEntity(ctx =>
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
            });
        }
    }

    private sealed class ProductDimensionType : ObjectType<ProductDimension>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductDimension> descriptor)
        {
            descriptor.Field(x => x.Size).Type<StringType>();
            descriptor.Field(x => x.Weight).Type<FloatType>();
        }
    }

    private sealed class ProductVariationType : ObjectType<ProductVariation>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductVariation> descriptor)
        {
            descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
        }
    }

    private sealed class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Key(x => x.Email);
            descriptor.Field(x => x.Email).Type<StringType>().External();
            descriptor.Field(x => x.TotalProductsCreated).Type<IntType>().External();
        }
    }

    private sealed class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(x => x.GetProduct(null!, null!))
                .Argument("id", x => x.Type<NonNullType<IdType>>())
                .Type<ProductType>();
        }
    }

    private sealed class ProductsRepository
    {
        private static readonly Product[] s_products =
        {
            new(
                Id: "apollo-federation",
                Sku: "federation",
                Package: "@apollo/federation",
                Variation: new ProductVariation("OSS"),
                Dimensions: new ProductDimension(Size: "1", Weight: 1),
                CreatedBy: new User("support@apollographql.com", TotalProductsCreated: 1991)),
            new(
                Id: "apollo-studio",
                Sku: "studio",
                Package: string.Empty,
                Variation: new ProductVariation("platform"),
                Dimensions: new ProductDimension(Size: "1", Weight: 1),
                CreatedBy: new User("support@apollographql.com", TotalProductsCreated: 1991)),
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

    private sealed class Query
    {
        public Product? GetProduct([Service] ProductsRepository products, string id)
        {
            return products.Find(id);
        }
    }

    private sealed record Product(
        string Id,
        string? Sku,
        string? Package,
        ProductVariation? Variation,
        ProductDimension? Dimensions,
        User? CreatedBy);

    private sealed record ProductDimension(string? Size, double? Weight);

    private sealed record ProductVariation(string? Id);

    private sealed record User(string? Email, int? TotalProductsCreated);
}