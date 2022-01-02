using System;
using System.Collections.Generic;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class EntityResolverKeyInterceptor : TypeInterceptor
{
    private readonly IEntityResolverRegistry _entityResolverRegistry;

    public EntityResolverKeyInterceptor(IEntityResolverRegistry entityResolverRegistry)
    {
        if (entityResolverRegistry is null)
        {
            throw new ArgumentNullException(nameof(entityResolverRegistry));
        }

        _entityResolverRegistry = entityResolverRegistry;
    }

    public override void OnAfterInitialize(
        ITypeDiscoveryContext _,
        DefinitionBase? definition,
        IDictionary<string, object?> __)
    {
        switch (definition)
        {
            case ObjectTypeDefinition:
            case InterfaceTypeDefinition:
                const string key = EntityResolverConfig.Names.InterceptorKey;
                if (definition.ContextData.TryGetValue<EntityResolverDelegate>(key, out var resolver))
                {
                    _entityResolverRegistry.Add(definition.Name, resolver!);
                }
                break;
        }
    }
}