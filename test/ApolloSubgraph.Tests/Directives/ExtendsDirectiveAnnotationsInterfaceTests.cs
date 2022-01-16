using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class ExtendsDirectiveAnnotationsInterfaceTests
{
    [Fact]
    public async Task When_extends_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddType<IProduct>();
            builder.AddType<Product>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "extends"),
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [GraphQLExtends]
    public interface IProduct
    {
        [GraphQLKey]
        public string Upc { get; }
    }

    [GraphQLExtends]
    public class Product : IProduct
    {
        public Product(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        public string Upc { get; }
    }
}