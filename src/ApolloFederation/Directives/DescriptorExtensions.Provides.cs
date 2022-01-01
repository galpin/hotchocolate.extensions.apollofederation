using System;
using HotChocolate.Language;
using HotChocolate.Types;
using static HotChocolate.Extensions.ApolloFederation.Properties.FederationResources;

namespace HotChocolate.Extensions.ApolloFederation;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Adds the <c>@provides</c> directive that is used to annotate the expected returned field set from a field on
    /// a base type that is guaranteed to be selectable by the gateway.
    /// </summary>
    /// <param name="descriptor">The object field descriptor on which this directive shall be annotated.</param>
    /// <param name="fieldSet">The field set that is guaranteed to be selectable by the gateway.</param>
    /// <returns>The object field descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fieldSet"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IObjectFieldDescriptor Provides(this IObjectFieldDescriptor descriptor, string fieldSet)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }
        if (string.IsNullOrWhiteSpace(fieldSet))
        {
            throw new ArgumentException(
                FieldDescriptorExtensions_Provides_FieldSet_CannotBeNullOrEmpty,
                nameof(fieldSet));
        }

        return descriptor.Directive(
            ProvidesDirectiveType.Names.Provides,
            new ArgumentNode(ProvidesDirectiveType.Names.Fields, fieldSet));
    }
}
