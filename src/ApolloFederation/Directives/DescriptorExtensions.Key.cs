using System;
using HotChocolate.Language;
using HotChocolate.Types;
using static HotChocolate.Extensions.ApolloFederation.Properties.FederationResources;

namespace HotChocolate.Extensions.ApolloFederation;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Adds the <c>@key</c> directive that indicates fields that can be used to uniquely identify and fetch an
    /// object or interface.
    /// </summary>
    /// <param name="descriptor">The object type descriptor on which this directive shall be annotated.</param>
    /// <param name="fieldSet">The fields that uniquely identify the object.</param>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fieldSet"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IObjectTypeDescriptor Key(this IObjectTypeDescriptor descriptor, string fieldSet)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }
        if (string.IsNullOrWhiteSpace(fieldSet))
        {
            throw new ArgumentException(
                FieldDescriptorExtensions_Key_FieldSet_CannotBeNullOrEmpty,
                nameof(fieldSet));
        }

        return descriptor.Directive(
            KeyDirectiveType.Names.Key,
            new ArgumentNode(KeyDirectiveType.Names.Fields, fieldSet));
    }

    /// <summary>
    /// Adds the <c>@key</c> directive which indicates fields that can be used to uniquely identify and fetch an
    /// object or interface.
    /// </summary>
    /// <param name="descriptor">The object type descriptor on which this directive shall be annotated.</param>
    /// <param name="fieldSet">The fields that uniquely identify the object.</param>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fieldSet"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IObjectTypeDescriptor<T> Key<T>(this IObjectTypeDescriptor<T> descriptor, string fieldSet)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }
        if (string.IsNullOrWhiteSpace(fieldSet))
        {
            throw new ArgumentException(
                FieldDescriptorExtensions_Key_FieldSet_CannotBeNullOrEmpty,
                nameof(fieldSet));
        }

        return descriptor.Directive(
            KeyDirectiveType.Names.Key,
            new ArgumentNode(KeyDirectiveType.Names.Fields, fieldSet));
    }

    /// <summary>
    /// Adds the <c>@key</c> directive which indicates fields that can be used to uniquely identify and fetch an
    /// object or interface.
    /// </summary>
    /// <param name="descriptor">The object field descriptor on which this directive shall be annotated.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    public static IObjectFieldDescriptor Key(this IObjectFieldDescriptor descriptor)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        descriptor.Extend().OnBeforeCreate(d => d.ContextData[GraphQLKeyAttribute.Names.Marker] = true);
        return descriptor;
    }
}