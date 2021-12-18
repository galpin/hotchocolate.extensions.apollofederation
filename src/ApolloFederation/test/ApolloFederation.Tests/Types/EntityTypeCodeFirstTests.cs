using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Types;

public class EntityTypeCodeFirstTests
{
    [Fact]
    public async Task When_key_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product").Key("id");
                x.Field("id").Type<IntType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name));
    }

    [Fact]
    public async Task When_key_is_specified_on_multiple_objects()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product").Key("id");
                x.Field("id").Type<IntType>();
            });
            builder.AddObjectType(x =>
            {
                x.Name("Review");
                x.Field("id").Key().Type<IntType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<EntityType>("_Entity");

        Assert.Collection(
            sut.Types.Values,
            x => Assert.Equal("Product", x.Name),
            x => Assert.Equal("Review", x.Name));
    }
}