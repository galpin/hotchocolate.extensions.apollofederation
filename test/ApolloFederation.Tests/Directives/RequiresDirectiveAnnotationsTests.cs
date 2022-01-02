using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class RequiresDirectiveAnnotationsTests
{
    [Fact]
    public async Task Ctor_correctly_configures_directive()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType<Review>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>(nameof(Review));

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"id\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    public class Review
    {
        public Review(int id, Product product)
        {
            Id = id;
            Product = product;
        }

        [GraphQLKey]
        public int Id { get; }

        [GraphQLRequires("id")]
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