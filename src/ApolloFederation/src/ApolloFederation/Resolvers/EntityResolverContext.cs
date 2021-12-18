using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class EntityResolverContext : IEntityResolverContext
{
    public EntityResolverContext(IServiceProvider services, IReadOnlyDictionary<string, object?> representation)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        if (representation is null)
        {
            throw new ArgumentNullException(nameof(representation));
        }

        Services = services;
        Representation = representation;
    }

    public IServiceProvider Services { get; }

    public IReadOnlyDictionary<string, object?> Representation { get; }

    public T Service<T>()
    {
        return Services.GetRequiredService<T>();
    }
}