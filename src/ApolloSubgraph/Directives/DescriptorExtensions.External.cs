using System;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloSubgraph;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Specifies the <c>@external</c> directive for the field.
    /// </summary>
    /// <param name="descriptor">The object field descriptor.</param>
    /// <returns>The object field descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectFieldDescriptor External(this IObjectFieldDescriptor descriptor)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        return descriptor.Directive(new ExternalDirectiveType());
    }

    /// <summary>
    /// Specifies the <c>@external</c> directive for the field.
    /// </summary>
    /// <param name="descriptor">The interface field descriptor.</param>
    /// <returns>The interface field descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    public static IInterfaceFieldDescriptor External(this IInterfaceFieldDescriptor descriptor)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        return descriptor.Directive(new ExternalDirectiveType());
    }
}