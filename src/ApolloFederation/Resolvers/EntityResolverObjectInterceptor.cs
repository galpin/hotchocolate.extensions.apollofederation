using System;
using System.Collections.Generic;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors.Definitions;
using static HotChocolate.Extensions.ApolloFederation.EntityResolverConfig.Names;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class EntityResolverObjectInterceptor : TypeInterceptor
{
    private readonly IEntityResolverRegistry _entityResolverRegistry;

    public EntityResolverObjectInterceptor(IEntityResolverRegistry entityResolverRegistry)
    {
        if (entityResolverRegistry is null)
        {
            throw new ArgumentNullException(nameof(entityResolverRegistry));
        }

        _entityResolverRegistry = entityResolverRegistry;
    }

    public override void OnAfterInitialize(
        ITypeDiscoveryContext context,
        DefinitionBase? definition,
        IDictionary<string, object?> _)
    {
        if (definition is ObjectTypeDefinition objectDefinition)
        {
            if (objectDefinition.ContextData.TryGetValue<EntityResolverDelegate>(InterceptorKey, out var resolver))
            {
                _entityResolverRegistry.Add(definition.Name, resolver!);
            }
        }
    }
}