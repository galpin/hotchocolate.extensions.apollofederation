using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class ProvidesDirectiveAnnotationsInterfaceTests
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
            sut.Fields["products"].Directives,
            x => AssertEx.Directive(x, "provides", ("fields", "\"name\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [GraphQLExtends]
    public interface IReview
    {
        [GraphQLKey]
        [GraphQLExternal]
        int Id { get; }

        [GraphQLProvides("name")]
        IReadOnlyList<Product> Products { get; }
    }

    public class Review : IReview
    {
        public Review(int id, IReadOnlyList<Product> products)
        {
            Id = id;
            Products = products;
        }

        [GraphQLKey]
        [GraphQLExternal]
        public int Id { get; }

        [GraphQLProvides("name")]
        public IReadOnlyList<Product> Products { get; }
    }

    [GraphQLExtends]
    public class Product
    {
        public Product(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }
    }
}