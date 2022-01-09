# HotChocolate.Extensions.ApolloSubgraph :coffee: :rocket:

![Nuget](https://img.shields.io/nuget/v/HotChocolate.Extensions.ApolloSubgraph) ![GitHub](https://img.shields.io/github/license/galpin/hotchocolate.extensions.apollosubgraph)

This package extends the [HotChocolate GraphQL server][HotChocolate] with support for building
[Apollo Federation][ApolloFederation] subgraphs.

## Getting Started

Add the Apollo Federation support to the schema by using the `AddApolloSubgraph` extension method when configuring services
during start-up, e.g.

```csharp
services.AddGraphQLServer()
  .AddApolloSubgraph();
```

The package supports all three paradigms for writing subgraphs in HotChocolate.

The following examples are based on the [Apollo reviews subgraph][ApolloReviews] schema.

### Example User Type (SDL)

```graphql
type User @extends @key(fields: "id") {
    id: ID! @external
    username: String @external
    reviews: [Review]
}
```

### Code-first

The code-first approach uses extension methods to specify directives using descriptors and a func-based entity resolver.

* The object-type can specify the `@extends` and `@key` directives using the `Extends` and `Key` extension methods.

* The field-type can specify the `@key`, `@provides` and `@requires` directives using the `Key`, `Provides` and
`Requires` extension methods.

* The reference resolver for the entity can be specified using the `ResolveEntity` extension method.

#### Example User Type (Code-first)

```csharp
public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Extends();
        descriptor.Key(x => x.Id);
        descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
        descriptor.Field(x => x.Username).Type<StringType>().External();
        descriptor.Field(x => x.GetReviews(default)).Type<ListType<ReviewType>>();
        descriptor.ResolveEntity(x => new User(x.Representation.GetValue<string>("id")));
    }
}
```

See [CodeFirstTest.cs](test/ApolloSubgraph.Tests/Integration/Reviews/CodeFirstTest.cs) for a complete example.

#### Type Extensions

The federation directives and entity resolver can also be specified using a type extension if you cannot modify the
original types.

```csharp
public class UserTypeExtension : ObjectTypeExtension<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Extends();
        descriptor.Key(x => x.Id);
        descriptor.Field(x => x.Username).External();
        descriptor.ResolveEntity(x => new User(x.Representation.GetValue<string>("id")));
    }
}
```

### Annotations-based

The annotations-based approach uses attributes to decorate types and a convention-based entity resolver method.

* The type can specify the `@extends` and `@key` directives using the `GraphQLExtends` and `GraphQLKey` attributes.

* The type properties can specify the `@key`, `@provides` and `@requires` directives using the `GraphQLKey`,
  `GraphQLProvides` and `GraphQLRequires` attributes.

* The reference resolver for the entity is be specified by adding a `ResolveEntity` (returns `T`) or
  `ResolveEntityAsync` (returns `Task<T>`) method to the type.

  If you want to use a different method name or prefer to be explicit, the `EntityResolver` attribute can be used
  to annotate method with a matching signature.

#### Example User Type (Annotations-based)

```csharp
[GraphQLExtends]
public sealed record User
{
    public User(string id, string username)
    {
        Id = id;
        Username = username;
    }

    [GraphQLKey]
    [GraphQLExternal]
    [GraphQLType(typeof(IdType))]
    public string Id { get; }

    [GraphQLExternal]
    public string Username { get; }

    public IReadOnlyList<Review> GetReviews()
    {
        return Array.Empty<Review>();
    }

    public static User? ResolveEntity(IEntityResolverContext context)
    {
        return new User(context.Representation.GetValue<string>("id"));
    }
}
```

See [AnnotationsTest.cs](test/ApolloSubgraph.Tests/Integration/Reviews/AnnotationsTest.cs) for a complete example.

#### Type Extensions

The federation directives and entity resolver can also be specified using a type extension if you cannot modify the
original types.

```csharp
[ExtendObjectType(typeof(User))]
[GraphQLExtends]
[GraphQLKey("upc")]
[GraphQLExternal("username")]
public class UserExtensions
{
    public static User ResolveEntity(IEntityResolverContext context)
    {
        return new User(context.Representation.GetValue<string>("id"));
    }
}
```

### Schema-first

The schema-first approach uses attributes to decorate bound types and a func-based entity resolver.

* The `@extends`, `@key`, `@provides` and `@requires` directives are specified in the schema for object-types
  and fields.

* The reference resolver for the entity can be specified using the `AddEntityResolver` extension method or by using
  the convention-based resolver method (see [Annotations-based](#annotations-based)).

:warning: Use the `@extends` directive for types that are defined in another subgraph (`extend type` is not supported)!

#### Example User Type (Schema-first)

```csharp
services.AddGraphQLServer()
  .AddApolloSubgraph()
  .AddDocumentFromString(@"
      type User @extends @key(fields: ""id"") {
          id: ID! @external
          username: String @external
          reviews: [Review]
      }

      type Review
      type Query
  ")
  .BindRuntimeType<User>()
  .AddEntityResolver(x => new User(x.Representation.GetValue<string>("id")));

public record User(string Id, string Username = null)
{
    public IReadOnlyList<Review> GetReviews()
    {
        return Array.Empty<Review>();
    }
}
```

The convention-based entity resolver will be used if a `ResolveEntity` or `ResolveEntityAsync` method is added to the
bound type.

```csharp
public record User(string Id, string Username = null)
{
    public IReadOnlyList<Review> GetReviews()
    {
        return Array.Empty<Review>();
    }

    public static User? ResolveEntity(IEntityResolverContext context)
    {
        return new User(context.Representation.GetValue<string>("id"));
    }
}
```

See [SchemaFirstTest.cs](test/ApolloSubgraph.Tests/Integration/Reviews/SchemaFirstTest.cs) for a complete example.

## Entity Resolvers

The entity resolver (also known as [a reference resolver][ApolloReferenceResolver]) enables the gateway to resolve an
entity by its `@key` fields.

Entity resolvers are represented by the `EntityResolverDelegate` and are invoked with a context that provides access
to the representation specified in the query to the `_entities` field.

The delegate is asynchronous but overloads are also provided for resolvers which are synchronous.

The convention-based method supports either `ResolveEntity` (returns `T`) and `ResolveEntityAsync` (returns `Task<T>`)
signatures, or any method name if a matching method is annotated with the `EntityResolver` attribute.

[HotChocolate]: https://github.com/ChilliCream/hotchocolate
[ApolloFederation]: https://www.apollographql.com/docs/federation/
[ApolloReviews]: https://www.apollographql.com/docs/federation/#subgraph-schemas
[ApolloReferenceResolver]: https://www.apollographql.com/docs/federation/entities/#resolving
