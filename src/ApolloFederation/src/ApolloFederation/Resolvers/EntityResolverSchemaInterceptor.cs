using System;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class EntityResolverSchemaInterceptor : SchemaInterceptor
{
    private readonly IEntityResolverRegistry _entityResolverRegistry;

    public EntityResolverSchemaInterceptor(IEntityResolverRegistry entityResolverRegistry)
    {
        if (entityResolverRegistry is null)
        {
            throw new ArgumentNullException(nameof(entityResolverRegistry));
        }

        _entityResolverRegistry = entityResolverRegistry;
    }

    public override void OnBeforeCreate(IDescriptorContext _, ISchemaBuilder schemaBuilder)
    {
        foreach (var config in schemaBuilder.GetOrAddEntityResolvers())
        {
            _entityResolverRegistry.Add(config.TypeName, config.Resolver);
        }
    }
}