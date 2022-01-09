using System;
using System.Threading.Tasks;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloSubgraph;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Adds a resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The object type descriptor.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor ResolveEntity<TRuntimeType>(
        this IObjectTypeDescriptor descriptor,
        Func<IEntityResolverContext, TRuntimeType?> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds a resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The interface type descriptor.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The interface type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IInterfaceTypeDescriptor ResolveEntity<TRuntimeType>(
        this IInterfaceTypeDescriptor descriptor,
        Func<IEntityResolverContext, TRuntimeType?> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds an asynchronous resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The object type descriptor.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor ResolveEntity<TRuntimeType>(
        this IObjectTypeDescriptor descriptor,
        Func<IEntityResolverContext, Task<TRuntimeType?>> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds an asynchronous resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The interface type descriptor.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The interface type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IInterfaceTypeDescriptor ResolveEntity<TRuntimeType>(
        this IInterfaceTypeDescriptor descriptor,
        Func<IEntityResolverContext, Task<TRuntimeType?>> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds a resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The object type descriptor.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The object type.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor<TRuntimeType> ResolveEntity<TRuntimeType>(
        this IObjectTypeDescriptor<TRuntimeType> descriptor,
        Func<IEntityResolverContext, TRuntimeType?> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds a resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The interface type descriptor.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The interface type.</typeparam>
    /// <returns>The interface type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IInterfaceTypeDescriptor<TRuntimeType> ResolveEntity<TRuntimeType>(
        this IInterfaceTypeDescriptor<TRuntimeType> descriptor,
        Func<IEntityResolverContext, TRuntimeType?> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds an asynchronous resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The object type descriptor.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor<TRuntimeType> ResolveEntity<TRuntimeType>(
        this IObjectTypeDescriptor<TRuntimeType> descriptor,
        Func<IEntityResolverContext, Task<TRuntimeType?>> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds an asynchronous resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The interface type descriptor.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TRuntimeType">The type of the entity to resolve.</typeparam>
    /// <returns>The interface type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IInterfaceTypeDescriptor<TRuntimeType> ResolveEntity<TRuntimeType>(
        this IInterfaceTypeDescriptor<TRuntimeType> descriptor,
        Func<IEntityResolverContext, Task<TRuntimeType?>> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    private static void AddEntityResolver<T>(this IDescriptor<T> descriptor, EntityResolverDelegate resolver)
        where T : DefinitionBase
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        descriptor.SetContextData(EntityResolverConfig.Names.InterceptorKey, resolver);
    }
}