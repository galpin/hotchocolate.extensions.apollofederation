using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class ExternalDirectiveCodeFirstTests
{
    [Fact]
    public async Task When_external_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Key().Type<NonNullType<StringType>>();
                x.Field("id").External().Type<IntType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Fields["id"].Directives,
            x => Assert.Equal("external", x.Name));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_external_is_specified_on_object_extension()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Type<NonNullType<StringType>>();
                x.Field("id").Type<IntType>();
            });
            builder.AddTypeExtension<ProductTypeExtension>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Product");

        Assert.Collection(
            sut.Fields["id"].Directives,
            x => Assert.Equal("external", x.Name));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    private sealed class ProductTypeExtension : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Product");
            descriptor.Key("upc");
            descriptor.Field("id").External();
        }
    }
}