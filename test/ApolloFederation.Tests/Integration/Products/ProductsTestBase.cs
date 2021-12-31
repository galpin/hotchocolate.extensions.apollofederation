using System.Threading.Tasks;
using Xunit;

namespace HotChocolate.Extensions.ApolloFederation.Integration.Products;

/// <summary>
/// https://github.com/apollographql/apollo-federation-subgraph-compatibility
/// </summary>
public abstract class ProductsTestBase : IntegrationTestBase
{
    [Fact]
    public async Task Service_sdl()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _service {
                sdl
            }
        }
        ");
    }

    [Fact]
    public async Task Entities_when_id_representation()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { __typename: ""Product"", id: ""apollo-federation"" }
            ]) {
                ...on Product {
                    sku
                }
            }
        }
        ");
    }

    [Fact]
    public async Task Entities_when_sku_and_package_representation()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { __typename: ""Product"", sku: ""federation"", package: ""@apollo/federation"" }
            ]) {
                ...on Product {
                    sku
                }
            }
        }
        ");
    }

    [Fact]
    public async Task Entities_when_sku_and_variation_representation()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { __typename: ""Product"", sku: ""federation"", variation: { id: ""OSS"" } }
            ]) {
                ...on Product {
                    sku
                }
            }
        }
        ");
    }

    [Fact]
    public async Task Query_product_when_provides_directive()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            product(id: ""apollo-federation"") {
                createdBy {
                    email
                    totalProductsCreated
                }
            }
        }
        ");
    }

    [Fact]
    public async Task Query_product_when_requires_directive()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            product(id: ""apollo-federation"") {
                dimensions {
                    size
                    weight
                }
            }
        }
        ");
    }
}