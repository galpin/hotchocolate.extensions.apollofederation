using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph;

public class FederatedSchemaPrinterTests
{
    [Theory]
    [MemberData(nameof(Can_roundtrip_sdl_provider))]
    public async Task Can_roundtrip_sdl(Action<IRequestExecutorBuilder>? configure, string source, string expected)
    {
        var sut = new FederatedSchemaPrinter();
        var schema = await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(source);
            configure?.Invoke(builder);
            if (!expected.Contains("type Query"))
            {
                builder.AddQueryType();
            }
        });

        var actual = sut.Print(schema);

        AssertEx.Sdl(expected, actual);
    }

    private static IEnumerable<object[]> Can_roundtrip_sdl_provider()
    {
        // Requires directive.
        yield return _(@"
             type Product @key(fields: ""upc"") {
                  name: String
             }

             type Review @key(fields: ""id"") {
                  id: Int
                  product: Product @requires(fields: ""id"")
             }");

        // Provides directive.
        yield return _(@"
             type Product @key(fields: ""upc"") {
                  upc: String @external
             }

             type Review @key(fields: ""id"") {
                  id: Int
                  product: Product @provides(fields: ""name"")
             }");

        // External directive.
        yield return _(@"
             type Product @extends @key(fields: ""id"") {
                 id: ID @external
             }");

        // Key directive.
        yield return _(@"
             interface IProduct @key(fields: ""id"") {
                 id: ID
             }

             type Product implements IProduct @key(fields: ""id"") {
                 id: ID
             }");

        // Repeated key directives.
        yield return _(@"
             type Product @key(fields: ""id"") @key(fields: ""upc"") {
                 id: ID
                 upc: ID
             }");

        // Extends directive.
        yield return _(@"
             type Product @extends @key(fields: ""id"") {
                 id: ID
             }");

        // Query type.
        yield return _(@"
             type Product @key(fields: ""id"") {
                 id: ID
             }

             type Query {
                 products: [Product]
             }");

        // Enums.
        yield return _(@"
             type Product @key(fields: ""id"") {
                 id: ID
                 status: ProductStatus
             }

             enum ProductStatus {
                 IN_STOCK
                 OUT_OF_STOCK
             }");

        // Unions.
        yield return _(@"
             type Purchase @key(fields: ""id"") {
                 id: ID
             }

             type Refund @key(fields: ""id"") {
                 id: ID
             }

             union Transactions = Purchase | Refund");

        // Inputs.
        yield return _(@"
             type Product @key(fields: ""id"") {
                 id: ID
             }

             type Query {
                 products(filter: ProductFilter!): [Product]
             }

             input ProductFilter {
                 id: ID
             }");

        // Interfaces.
        yield return _(@"
             interface IProvideId {
                 id: ID
             }

             type Product implements IProvideId @key(fields: ""id"") {
                 id: ID
             }");

        // Custom scalars.
        yield return _(
            @"
            type Product @key(fields: ""id"") {
              id: CustomID
            }

            scalar CustomID",
            configure: x => x.AddType<CustomIdType>());

        // Documentation descriptions.
        yield return _(@"
             ""Interface description.""
             interface ITransaction {
                 ""Field description.""
                 id: ID
             }

             ""Type description.""
             type Purchase implements ITransaction @key(fields: ""kind"") {
                 ""Field description.""
                 id: ID
             }

             ""Query description.""
             type Query {
                 ""Query field description.""
                 transactions: [ITransaction]
             }

             ""Type description.""
             type Refund implements ITransaction @key(fields: ""id"") {
                 ""Field description.""
                 id: ID
             }

             ""Union description.""
             union Transactions = Purchase | Refund

             ""Input description.""
             input TransactionFilter {
                 ""Field description.""
                 id: ID
             }

             ""Enum description.""
             enum TransactionType {
                 ""Enum value description 1.""
                 PURCHASE
                 ""Enum value description 2.""
                 REFUND
             }");

        // Deprecated.
        yield return _(
            @"type Product @key(fields: ""id"") {
                id: ID @deprecated(reason: ""Field deprecated."")
            }

            type Query {
                products(id: ID): [Product] @deprecated(reason: ""Field deprecated."")
            }

             enum ProductStatus {
                 IN_STOCK
                 OUT_OF_STOCK
                 ON_ORDER @deprecated(reason: ""Enum value deprecated."")
             }");

        // Deprecated (no reason specified).
        yield return _(
            @"
            type Product @key(fields: ""id"") {
                id: ID @deprecated
            }",
            @"
            type Product @key(fields: ""id"") {
                id: ID @deprecated(reason: ""No longer supported."")
            }");

        // Input deprecations - not supported?
        // yield return _(@"
        //    type Product @key(fields: ""id"") {
        //        id: ID
        //    }

        //    type Query {
        //        products(
        //            filter: ProductFilter! @deprecated(reason: ""Argument deprecated."")
        //        ): [Product]
        //    }

        //   input ProductFilter {
        //      id: ID @deprecated(reason: ""Field deprecated."")
        //   }");

        object[] _(string source, string? expected = null, Action<IRequestExecutorBuilder>? configure = null)
        {
            source = source.Trim();
            return new object[] { configure!, source, expected ?? source };
        }
    }

    private sealed class CustomIdType : IdType
    {
        public CustomIdType()
            : base("CustomID")
        {
        }
    }
}