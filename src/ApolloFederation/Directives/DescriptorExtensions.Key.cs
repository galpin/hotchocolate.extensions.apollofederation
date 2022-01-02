using System;
using System.Linq.Expressions;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Utilities;

namespace HotChocolate.Extensions.ApolloFederation;

public static partial class DescriptorExtensions
{
    /// <summary>
    /// Specifies the <c>@key</c> directive for the object.
    /// </summary>
    /// <param name="descriptor">The object type descriptor.</param>
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
            throw new ArgumentException(nameof(fieldSet));
        }

        return descriptor.Directive(
            KeyDirectiveType.Names.Key,
            new ArgumentNode(KeyDirectiveType.Names.Fields, fieldSet));
    }

    /// <summary>
    /// Specifies the <c>@key</c> directive for the object.
    /// </summary>
    /// <param name="descriptor">The object type descriptor.</param>
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
            throw new ArgumentException(nameof(fieldSet));
        }

        return descriptor.KeyDirective(fieldSet);
    }

    /// <summary>
    /// Specifies the <c>@key</c> directive for the object.
    /// </summary>
    /// <param name="descriptor">The object type descriptor.</param>
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
    /// Specifies the field represents a <c>@key</c> directive for the object.
    /// </summary>
    /// <param name="descriptor">The object field descriptor.</param>
    /// <returns>The object field descriptor.</returns>
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