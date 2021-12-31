using System;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Provides extensions to <see cref="IObjectTypeDescriptor"/>.
/// </summary>
public static partial class DescriptorExtensions
{
    /// <summary>
    /// Adds the <c>@extends</c> directive that is used to indicate a type extension.
    /// </summary>
    /// <param name="descriptor">The object field descriptor on which this directive shall be annotated.</param>
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
    /// Adds the <c>@extends</c> directive that is used to indicate a type extension.
    /// </summary>
    /// <param name="descriptor">The object field descriptor on which this directive shall be annotated.</param>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectTypeDescriptor<T> Extends<T>(this IObjectTypeDescriptor<T> descriptor)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        return descriptor.Directive(ExtendsDirectiveType.Names.Extends);
    }
}