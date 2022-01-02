using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Directives;

public class KeyDirectiveSchemaFirstInterfaceTests
{
    [Fact]
    public async Task When_key_is_specified_on_interface()
    {
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                interface IProduct @key(fields: ""upc"") {
                     upc: String!
                     id: String!
                }

                type Product implements IProduct @key(fields: ""upc"") {
                     upc: String!
                     id: String!
                }

                type Query
            ");
        });

        var sut = schema.GetType<InterfaceType>("IProduct");

        Assert.Collection(
            sut.Directives,
            x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")));
        await schema.QuerySdlAndMatchSnapshotAsync();
    }

     [Fact]
     public async Task When_key_is_specified_on_interface_multiple_times()
     {
         var schema = await BuildSchemaAsync(builder =>
         {
             builder.AddDocumentFromString(@"
                interface IProduct @key(fields: ""upc"") @key(fields: ""id"") {
                     upc: String!
                     id: String!
                }

                type Product implements IProduct @key(fields: ""upc"") @key(fields: ""id"") {
                     upc: String!
                     id: String!
                }

                type Query
             ");
         });

         var sut = schema.GetType<InterfaceType>("IProduct");

         Assert.Collection(
             sut.Directives,
             x => AssertEx.Directive(x, "key", ("fields", "\"upc\"")),
             x => AssertEx.Directive(x, "key", ("fields", "\"id\"")));
         await schema.QuerySdlAndMatchSnapshotAsync();
     }
}