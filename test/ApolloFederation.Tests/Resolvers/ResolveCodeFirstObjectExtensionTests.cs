using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class ResolveCodeFirstObjectExtensionTests : ResolveTestBase
{
    [Fact]
    public async Task Resolve_when_immediate_resolver()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddType<ProductType>();
            builder.AddType<ProductTypeExtension>();
            builder.AddQueryType();
        });

        await QueryProductAndMatchSnapshotAsync(schema);
    }

    private sealed class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Field(x => x.Upc).Type<NonNullType<StringType>>();
        }
    }

    private sealed class ProductTypeExtension : ObjectTypeExtension<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Extends();
            descriptor.Key(x => x.Upc);
            descriptor.ResolveEntity(_ => new Product("1"));
        }
    }

    private sealed record Product(string Upc);
}