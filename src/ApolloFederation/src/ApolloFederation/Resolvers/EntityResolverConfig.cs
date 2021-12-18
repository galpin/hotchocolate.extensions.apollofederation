using System;

namespace HotChocolate.Extensions.ApolloFederation;

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

    public NameString TypeName { get; }

    public EntityResolverDelegate Resolver { get; }

    public static class Names
    {
        public const string InterceptorKey = "ApolloFederation.EntityResolver.InterceptorKey";
    }
}