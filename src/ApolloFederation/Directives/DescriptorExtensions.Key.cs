using System;
using System.Linq.Expressions;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Utilities;
using static HotChocolate.Extensions.ApolloFederation.Properties.FederationResources;

namespace HotChocolate.Extensions.ApolloFederation;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Adds the <c>@key</c> directive that indicates fields that can be used to uniquely identify and fetch an
    /// object or interface.
    /// </summary>
    /// <param name="descriptor">The object type descriptor on which this directive shall be annotated.</param>
    /// <param name="fieldSet">The field set that uniquely identifies the object.</param>
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
    /// <param name="fieldSet">The field set that uniquely identifies the object.</param>
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

        return descriptor.KeyDirective(fieldSet);
    }

    /// <summary>
    /// Adds the <c>@key</c> directive which indicates fields that can be used to uniquely identify and fetch an
    /// object or interface.
    /// </summary>
    /// <param name="descriptor">The object type descriptor on which this directive shall be annotated.</param>
    /// <param name="propertyOrMethodName">
    /// The expression selecting a property or method of <typeparamref name="T" />.
    /// </param>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="propertyOrMethodName"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IObjectTypeDescriptor<T> Key<T, TPropertyType>(
        this IObjectTypeDescriptor<T> descriptor,
        Expression<Func<T, TPropertyType>> propertyOrMethodName)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }
        if (propertyOrMethodName is null)
        {
            throw new ArgumentNullException(nameof(propertyOrMethodName));
        }

        var naming = descriptor.Extend().Context.Naming;
        var member = propertyOrMethodName.ExtractMember();
        var fieldName = naming.GetMemberName(member, MemberKind.ObjectField);
        return descriptor.KeyDirective(fieldName);
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

        descriptor.SetContextData(GraphQLKeyAttribute.Names.InterceptorKey, true);
        return descriptor;
    }

    private static IObjectTypeDescriptor<T> KeyDirective<T>(this IObjectTypeDescriptor<T> descriptor, string fieldSet)
    {
        return descriptor.Directive(
            KeyDirectiveType.Names.Key,
            new ArgumentNode(KeyDirectiveType.Names.Fields, fieldSet));
    }
}