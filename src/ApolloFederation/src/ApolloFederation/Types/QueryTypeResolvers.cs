using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Resolvers;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class QueryTypeResolvers
{
    private readonly IEntityResolverRegistry _entityResolverRegistry;

    public QueryTypeResolvers(IEntityResolverRegistry entityResolverRegistry)
    {
        if (entityResolverRegistry == null)
        {
            throw new ArgumentNullException(nameof(entityResolverRegistry));
        }

        _entityResolverRegistry = entityResolverRegistry;
    }

    public async Task<IReadOnlyList<object?>> GetEntitiesAsync(IResolverContext context)
    {
        var representations = context.ArgumentValue<object[]>(QueryTypeExtension.Names.Representations);
        var entities = new List<object?>(representations.Length);
        foreach (IReadOnlyDictionary<string, object?> representation in representations)
        {
            entities.Add(await ResolveAsync(representation).ConfigureAwait(false));
        }
        return entities;

        ValueTask<object?> ResolveAsync(IReadOnlyDictionary<string, object?> representation)
        {
            if (!representation.TryGetValue("__typename", out var value))
            {
                throw ThrowHelper.Entities_Representation_Typename_Missing();
            }
            if (value is not string name)
            {
                throw ThrowHelper.Entities_Representation_Typename_Invalid(value);
            }
            if (!_entityResolverRegistry.TryGet(name, out var resolver))
            {
                throw ThrowHelper.Entities_Representation_Entity_NotFound(name);
            }
            return resolver!(new EntityResolverContext(context.Services, representation));
        }
    }
}