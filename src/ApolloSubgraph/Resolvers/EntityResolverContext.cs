using System;
using System.Collections.Generic;
using System.Threading;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloSubgraph;

internal sealed class EntityResolverContext : IEntityResolverContext
{
    public EntityResolverContext(IResolverContext fieldContext, IReadOnlyDictionary<string, object?> representation)
    {
        if (fieldContext is null)
        {
            throw new ArgumentNullException(nameof(fieldContext));
        }
        if (representation is null)
        {
            throw new ArgumentNullException(nameof(representation));
        }

        FieldContext = fieldContext;
        Representation = representation;
    }

    public IResolverContext FieldContext { get; }

    public IServiceProvider Services => FieldContext.Services;

    public IReadOnlyDictionary<string, object?> Representation { get; }

    public CancellationToken RequestAborted => FieldContext.RequestAborted;

    public T Service<T>()
    {
        return Services.GetRequiredService<T>();
    }
}