using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class RequiresDirectiveAnnotationsInterfaceTests
{
    [Fact]
    public async Task When_provides_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddType<IReview>();
            builder.AddType<Review>();
            builder.AddType<Product>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IReview));

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    public interface IReview
    {
        [GraphQLKey]
        int Id { get; }

        [GraphQLRequires("upc")]
        Product Product { get; }
    }

    public class Review : IReview
    {
        public Review(int id, Product product)
        {
            Id = id;
            Product = product;
        }

        [GraphQLKey]
        public int Id { get; }

        [GraphQLRequires("upc")]
        public Product Product { get; }
    }

    public class Product
    {
        public Product(string upc)
        {
            Upc = upc;
        }

        public string Upc { get; }
    }
}