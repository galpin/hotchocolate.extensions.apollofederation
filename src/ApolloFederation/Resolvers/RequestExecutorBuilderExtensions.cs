using System;
using System.Threading.Tasks;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Provides extensions to <see cref="IRequestExecutorBuilder"/>.
/// </summary>
public static class RequestExecutorBuilderExtensions
{
    /// <summary>
    /// Adds a resolver delegate for a specific entity.
    /// </summary>
    /// <param name="builder">The <see cref="IRequestExecutorBuilder"/>.</param>
    /// <param name="typeName">The name of the entity for which the resolver shall be added.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TEntity">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="builder"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IRequestExecutorBuilder AddEntityResolver<TEntity>(
        this IRequestExecutorBuilder builder,
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

        return builder.ConfigureSchema(x => x.AddEntityResolver(typeName, resolver));
    }

    /// <summary>
    /// Adds an asynchronous resolver delegate for a specific entity.
    /// </summary>
    /// <param name="builder">The <see cref="IRequestExecutorBuilder"/>.</param>
    /// <param name="typeName">The name of the entity for which the resolver shall be added.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TEntity">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="builder"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IRequestExecutorBuilder AddEntityResolver<TEntity>(
        this IRequestExecutorBuilder builder,
        NameString typeName,
        Func<IEntityResolverContext, Task<TEntity?>> resolver)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        return builder.ConfigureSchema(x => x.AddEntityResolver(typeName, resolver));
    }

    /// <summary>
    /// Adds a resolver delegate for a specific entity, inferring the entity name from the name of
    /// <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IRequestExecutorBuilder"/>.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TEntity">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="builder"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IRequestExecutorBuilder AddEntityResolver<TEntity>(
        this IRequestExecutorBuilder builder,
        Func<IEntityResolverContext, TEntity?> resolver)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        var typeName = typeof(TEntity).Name;
        return builder.AddEntityResolver(typeName, resolver);
    }

    /// <summary>
    /// Adds an asynchronous resolver delegate for a specific entity, inferring the entity name from the name of
    /// <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IRequestExecutorBuilder"/>.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TEntity">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="builder"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IRequestExecutorBuilder AddEntityResolver<TEntity>(
        this IRequestExecutorBuilder builder,
        Func<IEntityResolverContext, Task<TEntity?>> resolver)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        var typeName = typeof(TEntity).Name;
        return builder.AddEntityResolver(typeName, resolver);
    }
}