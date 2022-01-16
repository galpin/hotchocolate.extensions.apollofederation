using System;
using HotChocolate.Language;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloSubgraph;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Specifies the <c>@requires</c> directive for the field.
    /// </summary>
    /// <param name="descriptor">The object field descriptor.</param>
    /// <param name="fieldSet">The field set that is guaranteed to be selectable by the gateway.</param>
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

    /// <summary>
    /// Specifies the <c>@requires</c> directive for the field.
    /// </summary>
    /// <param name="descriptor">The interface field descriptor.</param>
    /// <param name="fieldSet">The field set that is guaranteed to be selectable by the gateway.</param>
    /// <returns>The interface field descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fieldSet"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IInterfaceFieldDescriptor Requires(this IInterfaceFieldDescriptor descriptor, string fieldSet)
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