using System;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloSubgraph;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Specifies the <c>@extends</c> directive for the object.
    /// </summary>
    /// <param name="descriptor">The object type descriptor.</param>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor Extends(this IObjectTypeDescriptor descriptor)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        return descriptor.Directive(ExtendsDirectiveType.Names.Extends);
    }

    /// <summary>
    /// Specifies the <c>@extends</c> directive for the object.
    /// </summary>
    /// <param name="descriptor">The object type descriptor.</param>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor<TRuntimeType> Extends<TRuntimeType>(
        this IObjectTypeDescriptor<TRuntimeType> descriptor)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        return descriptor.Directive(ExtendsDirectiveType.Names.Extends);
    }
}