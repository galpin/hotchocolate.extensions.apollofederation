using System;
using System.Threading.Tasks;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Provides extensions to <see cref="IRequestExecutorBuilder"/>.
/// </summary>
public static partial class RequestExecutorBuilderExtensions
{
    /// <summary>
    /// Adds a resolver delegate for the specified entity.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="typeName">The name of the entity for which the resolver shall be added.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="builder"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IRequestExecutorBuilder AddEntityResolver<TRuntimeType>(
        this IRequestExecutorBuilder builder,
        NameString typeName,
        Func<IEntityResolverContext, TRuntimeType> resolver)
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
    /// Adds an asynchronous resolver delegate for the specified entity.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="typeName">The name of the entity for which the resolver shall be added.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="builder"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IRequestExecutorBuilder AddEntityResolver<TRuntimeType>(
        this IRequestExecutorBuilder builder,
        NameString typeName,
        Func<IEntityResolverContext, Task<TRuntimeType?>> resolver)
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
    /// Adds a resolver delegate for the specified entity, inferring the entity name from
    /// <typeparamref name="TRuntimeType"/>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="builder"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IRequestExecutorBuilder AddEntityResolver<TRuntimeType>(
        this IRequestExecutorBuilder builder,
        Func<IEntityResolverContext, TRuntimeType?> resolver)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        var typeName = typeof(TRuntimeType).Name;
        return builder.AddEntityResolver(typeName, resolver);
    }

    /// <summary>
    /// Adds an asynchronous resolver delegate for the specified entity, inferring the entity from
    /// <typeparamref name="TRuntimeType"/>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="builder"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IRequestExecutorBuilder AddEntityResolver<TRuntimeType>(
        this IRequestExecutorBuilder builder,
        Func<IEntityResolverContext, Task<TRuntimeType?>> resolver)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        var typeName = typeof(TRuntimeType).Name;
        return builder.AddEntityResolver(typeName, resolver);
    }
}