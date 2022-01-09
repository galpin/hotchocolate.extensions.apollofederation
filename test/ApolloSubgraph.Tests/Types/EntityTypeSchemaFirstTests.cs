using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HotChocolate.Extensions.ApolloSubgraph.Types;

public class EntityTypeSchemaFirstTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await Test.BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Product @key(fields: ""id"") {
                    id: Int
                }

                type Query
            ");
        });

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name));
    }

    [Fact]
    public async Task When_key_is_specified_on_multiple_objects()
    {
        var schema = await Test.BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Product @key(fields: ""id"") {
                    id: Int
                }

                type Review @key(fields: ""id"") {
                    id: Int
                }

                type Query
            ");
        });

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name),
            x => Assert.Equal("Review", x.Name));
    }
}