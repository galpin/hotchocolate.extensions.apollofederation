# HotChocolate.Extensions.ApolloFederation ☕ 🚀

This package extends the [HotChocolate GraphQL server][HotChocolate] with support for [Apollo Federation][ApolloFederation].

## Getting Started

Add the Apollo Federation support to the schema by using the `AddApolloFederation` extension when configuring services
during start-up, e.g.

```csharp
services.AddGraphQLServer()
        .AddApolloFederation();
```

The package supports all three paradigms for writing subgraphs in HotChocolate. The test project includes the examples based on
the [Apollo reviews subgraph][ApolloReviews] schema:

* [Code-first](test/ApolloFederation.Tests/Integration/Reviews/CodeFirstTest.cs)
* [Schema-first](test/ApolloFederation.Tests/Integration/Reviews/SchemaFirstTest.cs)
* [Annotations-based](test/ApolloFederation.Tests/Integration/Reviews/AnnotationsTest.cs)

## Credits

The package was originally based on the erstwhile implementation in the HotChocolate repo. See [LICENSE]() for more
information.

[HotChocolate]: https://github.com/ChilliCream/hotchocolate
[ApolloFederation]: https://www.apollographql.com/docs/federation/
[ApolloReviews]: https://www.apollographql.com/docs/federation/#subgraph-schemas
