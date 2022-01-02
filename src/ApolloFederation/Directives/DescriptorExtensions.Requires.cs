using System;
using HotChocolate.Language;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Adds the <c>@requires</c> directive that is used to annotate the required input field set from a base type for
    /// a resolver.
    /// </summary>
    /// <param name="descriptor">The object field descriptor on which this directive shall be annotated.</param>
    /// <param name="fieldSet">The field sset that is required from a base type for a resolver.</param>
    /// <returns>The object field descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fieldSet"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IObjectFieldDescriptor Requires(this IObjectFieldDescriptor descriptor, string fieldSet)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }
        if (string.IsNullOrWhiteSpace(fieldSet))
        {
            throw new ArgumentException(nameof(fieldSet));
        }

        return descriptor.Directive(
            RequiresDirectiveType.Names.Requires,
            new ArgumentNode(RequiresDirectiveType.Names.Fields, fieldSet));
    }
}