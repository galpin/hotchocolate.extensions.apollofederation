using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class KeyDirectiveAnnotationsInterfaceTests
{
    [Fact]
    public async Task When_key_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddInterfaceType<IProductWhenKeyIsSpecifiedOnClass>();
            builder.AddObjectType(x => x.Name("Product").Field("upc").Key().Type<StringType>());
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>("IProduct");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, x.Name, ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_interface_multiple_times()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddInterfaceType<IProductWhenKeyIsSpecifiedOnClassMultipleTimes>();
            builder.AddObjectType(x => x.Name("Product").Field("upc").Key().Type<StringType>());
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>("IProduct");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")),
            x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_key_is_specified_on_property()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddInterfaceType<IProductWhenKeyIsSpecifiedOnProperty>();
            builder.AddObjectType(x => x.Name("Product").Field("upc").Key().Type<StringType>());
            builder.AddQueryType();
        });

        var sut = schema.GetType<InterfaceType>("IProduct");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [GraphQLKey("upc")]
    [GraphQLName("IProduct")]
    public interface IProductWhenKeyIsSpecifiedOnClass
    {
        string? Upc { get; set; }

        string? Id { get; set; }
    }

    [GraphQLKey("upc")]
    [GraphQLKey("id")]
    [GraphQLName("IProduct")]
    public interface IProductWhenKeyIsSpecifiedOnClassMultipleTimes
    {
        string? Upc { get; set; }

        string? Id { get; set; }
    }

    [GraphQLName("IProduct")]
    public interface IProductWhenKeyIsSpecifiedOnProperty
    {
        [GraphQLKey]
        string? Upc { get; set; }

        string? Id { get; set; }
    }
}