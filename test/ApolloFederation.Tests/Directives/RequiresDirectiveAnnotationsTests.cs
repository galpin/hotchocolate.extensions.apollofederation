using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

#nullable disable

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
        var sut = schema.GetType<ObjectType>("Review");

        Assert.Collection(
            sut.Fields["product"].Directives,
            x => AssertEx.Directive(x, "requires", ("fields", "\"id\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

    public class Review
    {
        [GraphQLKey]
        public int Id { get; set; }

        [GraphQLRequires("id")]
        public Product Product { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
    }
}