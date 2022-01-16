using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Snapshooter.Xunit;
using Xunit;

namespace HotChocolate.Extensions.ApolloSubgraph.Resolvers;

public class ResolveDataLoaderTests : ResolveTestBase
{
    private Context Ctx { get; } = new();

    [Fact]
    public async Task Resolve_when_data_loader()
    {
        Snapshot.FullName();
        Ctx.Repository.SetupGetAsync(
            new[] { "1", "2", "3" },
            new[]
            {
                new Product("1"),
                new Product("2"),
                new Product("3"),
            });
        var executor = await Ctx.BuildExecutorAsync();

        await executor.ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { __typename: ""Product"", upc: ""1"" },
                { __typename: ""Product"", upc: ""2"" },
                { __typename: ""Product"", upc: ""3"" }
            ]) {
                ...on Product {
                    __typename
                    upc
                }
            }
        }
        ");

        Ctx.Repository.VerifyGetAsync(Times.Once());
    }

    private sealed class Context
    {
        public Context()
        {
            Repository = new MockProductRepository();
        }

        public MockProductRepository Repository { get; }

        public async Task<IRequestExecutor> BuildExecutorAsync()
        {
            var services = new ServiceCollection()
                .AddSingleton(Repository.Object)
                .AddGraphQL()
                .AddApolloSubgraph()
                .AddQueryType()
                .AddType<Product>()
                .AddDataLoader<ProductDataLoader>();
            return await services.BuildRequestExecutorAsync();
        }
    }

    [GraphQLKey("upc")]
    public sealed class Product
    {
        public Product(string upc)
        {
            Upc = upc;
        }

        public string Upc { get; }

        public static Task<Product> ResolveEntityAsync(IEntityResolverContext ctx)
        {
            var loader = ctx.Service<ProductDataLoader>();
            var upc = ctx.Representation.GetValue<string>("upc");
            return loader.LoadAsync(upc);
        }
    }

    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetAsync(IEnumerable<string> upcs);
    }

    private sealed class ProductDataLoader : BatchDataLoader<string, Product>
    {
        private readonly IProductRepository _productRepository;

        public ProductDataLoader(
            IProductRepository productRepository,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _productRepository = productRepository;
        }

        protected override async Task<IReadOnlyDictionary<string, Product>> LoadBatchAsync(
            IReadOnlyList<string> keys,
            CancellationToken cancellationToken)
        {
            var results = await _productRepository.GetAsync(keys);
            return results.ToDictionary(x => x.Upc);
        }
    }

    private sealed class MockProductRepository
    {
        private readonly Mock<IProductRepository> _mock = new();

        public IProductRepository Object => _mock.Object;

        public void SetupGetAsync(IEnumerable<string> keys, IReadOnlyList<Product> returns)
        {
            _mock.Setup(x => x.GetAsync(It.Is<IEnumerable<string>>(x => x.SequenceEqual(keys))))
                .ReturnsAsync(returns);
        }

        public void VerifyGetAsync(Times times)
        {
            _mock.Verify(x => x.GetAsync(It.IsAny<IEnumerable<string>>()), times);
        }
    }
}