using System;

namespace HotChocolate.Extensions.ApolloSubgraph;

internal sealed class EntityResolverConfig
{
    public EntityResolverConfig(NameString typeName, EntityResolverDelegate resolver)
    {
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        TypeName = typeName;
        Resolver = resolver;
    }

    public EntityResolverConfig(Type? runtimeType, EntityResolverDelegate resolver)
    {
        if (runtimeType is null)
        {
            throw new ArgumentNullException(nameof(runtimeType));
        }
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        Resolver = resolver;
        RuntimeType = runtimeType;
    }

    public NameString? TypeName { get; }

    public Type? RuntimeType { get; }

    public EntityResolverDelegate Resolver { get; }

    public static class Names
    {
        public const string InterceptorKey = "ApolloSubgraph.EntityResolver.InterceptorKey";
    }
}