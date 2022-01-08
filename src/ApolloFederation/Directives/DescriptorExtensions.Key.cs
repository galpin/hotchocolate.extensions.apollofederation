using System;
using System.Linq.Expressions;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;
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
    /// Specifies the <c>@key</c> directive for the interface.
    /// </summary>
    /// <param name="descriptor">The interface type descriptor.</param>
    /// <param name="fieldSet">The field set that uniquely identifies the interface.</param>
    /// <returns>The interface type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fieldSet"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IInterfaceTypeDescriptor Key(this IInterfaceTypeDescriptor descriptor, string fieldSet)
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
    public static IObjectTypeDescriptor<TRuntimeType> Key<TRuntimeType>(
        this IObjectTypeDescriptor<TRuntimeType> descriptor,
        string fieldSet)
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
    /// Specifies the <c>@key</c> directive for the interface.
    /// </summary>
    /// <param name="descriptor">The interface type descriptor.</param>
    /// <param name="fieldSet">The field set that uniquely identifies the object.</param>
    /// <returns>The interface type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fieldSet"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IInterfaceTypeDescriptor<TRuntimeType> Key<TRuntimeType>(
        this IInterfaceTypeDescriptor<TRuntimeType> descriptor,
        string fieldSet)
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
    /// The expression selecting a property or method of <typeparamref name="TRuntimeType" />.
    /// </param>
    /// <returns>The object type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="propertyOrMethodName"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IObjectTypeDescriptor<TRuntimeType> Key<TRuntimeType, TPropertyOrMethod>(
        this IObjectTypeDescriptor<TRuntimeType> descriptor,
        Expression<Func<TRuntimeType, TPropertyOrMethod>> propertyOrMethodName)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }
        if (propertyOrMethodName is null)
        {
            throw new ArgumentNullException(nameof(propertyOrMethodName));
        }

        return descriptor.KeyDirective(GetFieldName(descriptor, propertyOrMethodName));
    }

    /// <summary>
    /// Specifies the <c>@key</c> directive for the interface.
    /// </summary>
    /// <param name="descriptor">The interface type descriptor.</param>
    /// <param name="propertyOrMethodName">
    /// The expression selecting a property or method of <typeparamref name="TRuntimeType" />.
    /// </param>
    /// <returns>The interface type descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="propertyOrMethodName"/> is <see langword="null"/> or consists solely of white-space.
    /// </exception>
    public static IInterfaceTypeDescriptor<TRuntimeType> Key<TRuntimeType, TPropertyOrMethod>(
        this IInterfaceTypeDescriptor<TRuntimeType> descriptor,
        Expression<Func<TRuntimeType, TPropertyOrMethod>> propertyOrMethodName)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }
        if (propertyOrMethodName is null)
        {
            throw new ArgumentNullException(nameof(propertyOrMethodName));
        }

        return descriptor.KeyDirective(GetFieldName(descriptor, propertyOrMethodName));
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

        descriptor.SetContextData(KeyDirectiveType.Names.InterceptorKey, true);
        return descriptor;
    }

    /// <summary>
    /// Specifies the field represents a <c>@key</c> directive for the interface.
    /// </summary>
    /// <param name="descriptor">The interface field descriptor.</param>
    /// <returns>The interface field descriptor.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <see langword="null"/>.
    /// </exception>
    public static IInterfaceFieldDescriptor Key(this IInterfaceFieldDescriptor descriptor)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        descriptor.SetContextData(KeyDirectiveType.Names.InterceptorKey, true);
        return descriptor;
    }

    private static IObjectTypeDescriptor<T> KeyDirective<T>(
        this IObjectTypeDescriptor<T> descriptor,
        string fieldSet)
    {
        return descriptor.Directive(
            KeyDirectiveType.Names.Key,
            new ArgumentNode(KeyDirectiveType.Names.Fields, fieldSet));
    }

    private static IInterfaceTypeDescriptor<T> KeyDirective<T>(
        this IInterfaceTypeDescriptor<T> descriptor,
        string fieldSet)
    {
        return descriptor.Directive(
            KeyDirectiveType.Names.Key,
            new ArgumentNode(KeyDirectiveType.Names.Fields, fieldSet));
    }

    private static NameString GetFieldName<TDefinition, TRuntimeType, TPropertyType>(
        IDescriptor<TDefinition> descriptor,
        Expression<Func<TRuntimeType, TPropertyType>> propertyOrMethodName) where TDefinition : DefinitionBase
    {
        var member = propertyOrMethodName.ExtractMember();
        return descriptor.Extend().Context.GetMemberName(member, MemberKind.ObjectField);
    }
}