using System;
using System.Collections.Generic;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class EntityResolverRegistry : IEntityResolverRegistry
{
    private readonly Dictionary<NameString, EntityResolverDelegate> _items = new();

    public void Add(NameString typeName, EntityResolverDelegate value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        _items.Add(typeName, value);
    }

    public bool TryGet(NameString typeName, out EntityResolverDelegate? result)
    {
        return _items.TryGetValue(typeName, out result);
    }
}