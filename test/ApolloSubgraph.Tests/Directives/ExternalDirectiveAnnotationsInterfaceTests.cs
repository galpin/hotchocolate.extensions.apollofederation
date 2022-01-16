using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Directives;

public class ExternalDirectiveAnnotationsInterfaceTests
{
    [Fact]
    public async Task When_external_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddType<IProduct>();
            builder.AddType<Product>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>(nameof(IProduct));

        Assert.Collection(
            sut.Fields["id"].Directives,
            x => Assert.Equal("external", x.Name));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    public interface IProduct
    {
        [GraphQLKey]
        string? Upc { get; set; }

        [GraphQLExternal]
        int? Id { get; set; }
    }

    public class Product : IProduct
    {
        [GraphQLKey]
        public string? Upc { get; set; }

        [GraphQLExternal]
        public int? Id { get; set; }
    }
}