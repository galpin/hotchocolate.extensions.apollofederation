using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class RequiresDirectiveCodeFirstTests
{
    [Fact]
    public async Task When_requires_is_specified_on_object()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Review").Key("id");
                x.Field("id").Type<IntType>();
                x.Field("products").Type("[Product]").Requires("upc");
            });
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Type<StringType>();
            });
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Review");

        Assert.Collection(
            sut.Fields["products"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    [Fact]
    public async Task When_requires_is_specified_on_object_extension()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddObjectType(x =>
            {
                x.Name("Review");
                x.Field("id").Type<IntType>();
                x.Field("products").Type("[Product]");
            });
            builder.AddObjectType(x =>
            {
                x.Name("Product");
                x.Field("upc").Type<StringType>();
            });
            builder.AddTypeExtension<ReviewTypeExtension>();
            builder.AddQueryType();
        });

        var sut = schema.GetType<ObjectType>("Review");

        Assert.Collection(
            sut.Fields["products"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    private sealed class ReviewTypeExtension : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Review");
            descriptor.Key("id");
            descriptor.Field("products").Requires("upc");
        }
    }
}