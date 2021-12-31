using System;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Adds the <c>@external</c> directive that is used to mark a field as owned by another service.
    /// </summary>
    /// <param name="descriptor">The object field descriptor on which this directive shall be annotated.</param>
    /// <returns>The field type descriptor.</returns>
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
}