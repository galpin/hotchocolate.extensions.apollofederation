using System.Collections.Generic;
using HotChocolate.Utilities.Introspection;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Provides utility methods for federated and built-in types.
/// </summary>
public static class FederatedTypes
{
    private static readonly HashSet<string> _typeNames = new()
    {
        AnyType.Names.Any,
        EntityType.Names.Entity,
        FieldSetType.Names.FieldSet,
        ServiceType.Names.Service
    };

    /// <summary>
    /// Indicates whether or not a specified type name is a federated or built-in type.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="name"/> is a federated or built-in type;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool IsBuiltInOrFederatedType(string name)
    {
        return BuiltInTypes.IsBuiltInType(name) || _typeNames.Contains(name);
    }
}