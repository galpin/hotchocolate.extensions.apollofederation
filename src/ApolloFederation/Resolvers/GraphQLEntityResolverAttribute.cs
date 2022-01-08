using System;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Indicates that the annotated method is a resolver for a federated type.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class GraphQLEntityResolverAttribute : Attribute
{
}
