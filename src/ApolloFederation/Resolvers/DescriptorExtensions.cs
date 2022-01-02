using System;
using System.Threading.Tasks;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloFederation;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Adds a resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The object type descriptor for which the resolver shall be added.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TReturn">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor ResolveEntity<TReturn>(
        this IObjectTypeDescriptor descriptor,
        Func<IEntityResolverContext, TReturn?> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds an asynchronous resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The object type descriptor for which the resolver shall be added.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="TReturn">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor ResolveEntity<TReturn>(
        this IObjectTypeDescriptor descriptor,
        Func<IEntityResolverContext, Task<TReturn?>> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds a resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The object type descriptor for which the resolver shall be added.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="T">The object type.</typeparam>
    /// <typeparam name="TReturn">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor<T> ResolveEntity<T, TReturn>(
        this IObjectTypeDescriptor<T> descriptor,
        Func<IEntityResolverContext, TReturn?> resolver)
    {
        descriptor.AddEntityResolver(EntityResolverDelegateFactory.Create(resolver));
        return descriptor;
    }

    /// <summary>
    /// Adds an asynchronous resolver delegate for the specified entity.
    /// </summary>
    /// <param name="descriptor">The object type descriptor for which the resolver shall be added.</param>
    /// <param name="resolver">The resolver delegate.</param>
    /// <typeparam name="T">The object type.</typeparam>
    /// <typeparam name="TReturn">The type of the entity to resolve.</typeparam>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="descriptor"/> or <paramref name="resolver"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor<T> ResolveEntity<T, TReturn>(
        this IObjectTypeDescriptor<T> descriptor,
        Func<IEntityResolverContext, Task<TReturn?>> resolver)
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