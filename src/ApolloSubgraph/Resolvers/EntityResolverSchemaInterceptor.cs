using System;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloSubgraph;

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

    public override void OnBeforeCreate(IDescriptorContext context, ISchemaBuilder schemaBuilder)
    {
        foreach (var config in schemaBuilder.GetOrAddEntityResolvers())
        {
            var typeName = config.TypeName ?? context.Naming.GetTypeName(config.RuntimeType!);
            _entityResolverRegistry.Add(typeName, config.Resolver);
        }
    }
}