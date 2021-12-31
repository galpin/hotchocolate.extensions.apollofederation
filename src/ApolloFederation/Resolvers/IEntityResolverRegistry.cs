using System;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Specifies a registry containing entity types and their corresponding resolvers.
/// </summary>
public interface IEntityResolverRegistry
{
    /// <summary>
    /// Add a resolver for an entity with the specified type name.
    /// </summary>
    /// <param name="typeName">The name of entity.</param>
    /// <param name="value">The entity resolver.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    void Add(NameString typeName, EntityResolverDelegate value);

    /// <summary>
    /// Tries to get the resolver with the specified entity type name.
    /// </summary>
    /// <param name="typeName">The name of entity.</param>
    /// <param name="result">
    /// When this method returns, contains the resolver associated with the name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the registry contains a resolver with the specified name;
    /// otherwise <see langword="false"/>.
    /// </returns>
    bool TryGet(NameString typeName, out EntityResolverDelegate? result);
}