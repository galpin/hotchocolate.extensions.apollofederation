using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotChocolate.Extensions.ApolloSubgraph;

internal static class SchemaBuilderExtensions
{
    public static ISchemaBuilder AddEntityResolver<TEntity>(
        this ISchemaBuilder builder,
        NameString typeName,
        Func<IEntityResolverContext, TEntity> resolver)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        return AddEntityResolverInternal(
            builder,
            typeName,
            EntityResolverDelegateFactory.Create(resolver));
    }

    public static ISchemaBuilder AddEntityResolver<TReturn>(
        this ISchemaBuilder builder,
        NameString typeName,
        Func<IEntityResolverContext, Task<TReturn?>> resolver)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        return AddEntityResolverInternal(
            builder,
            typeName,
            EntityResolverDelegateFactory.Create(resolver));
    }

    public static List<EntityResolverConfig> GetOrAddEntityResolvers(this ISchemaBuilder builder)
    {
        const string key = EntityResolverConfig.Names.InterceptorKey;
        if (builder.ContextData.TryGetValue<List<EntityResolverConfig>>(key, out var result))
        {
            return result!;
        }
        result = new List<EntityResolverConfig>();
        builder.ContextData.Add(key, result);
        return result;
    }

    private static ISchemaBuilder AddEntityResolverInternal(
        ISchemaBuilder builder,
        NameString typeName,
        EntityResolverDelegate resolver)
    {
        var resolvers = builder.GetOrAddEntityResolvers();
        resolvers.Add(new EntityResolverConfig(typeName, resolver));
        return builder;
    }
}