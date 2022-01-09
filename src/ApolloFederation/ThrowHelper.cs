using System;
using System.Reflection;
using HotChocolate.Types;
using static HotChocolate.Extensions.ApolloFederation.Properties.FederationResources;

namespace HotChocolate.Extensions.ApolloFederation;

internal static class ThrowHelper
{
    public static SerializationException FieldSet_InvalidFormat(FieldSetType fieldSetType)
    {
        return new SerializationException(
            ErrorBuilder.New()
                .SetMessage(ThrowHelper_FieldSet_HasInvalidFormat)
                .SetCode(ErrorCodes.Scalars.InvalidSyntaxFormat)
                .Build(),
            fieldSetType);
    }

    public static SerializationException FieldSet_CannotParseValue(FieldSetType fieldSetType, Type valueType)
    {
        return new SerializationException(
            ErrorBuilder.New()
                .SetMessage(
                    ThrowHelper_FieldSet_CannotParseValue,
                    fieldSetType.Name,
                    valueType.FullName ?? valueType.Name)
                .SetCode(ErrorCodes.Scalars.InvalidRuntimeType)
                .Build(),
            fieldSetType);
    }

    public static SchemaException Key_FieldSet_CannotBeEmpty(Type type)
    {
        return new SchemaException(
            SchemaErrorBuilder.New()
                .SetMessage(
                    ThrowHelper_Key_FieldSet_CannotBeEmpty,
                    type.FullName ?? type.Name)
                .Build());
    }

    public static SchemaException External_FieldSet_CannotBeEmpty(Type type)
    {
        return new SchemaException(
            SchemaErrorBuilder.New()
                .SetMessage(
                    ThrowHelper_External_FieldSet_CannotBeEmpty,
                    type.FullName ?? type.Name)
                .Build());
    }

    public static SchemaException Provides_Field_CannotBeEmpty(MemberInfo member)
    {
        return new SchemaException(
            SchemaErrorBuilder.New()
                .SetMessage(ThrowHelper_Provides_Field_CannotBeEmpty)
                .SetExtension(nameof(member), member)
                .Build());
    }

    public static SchemaException Provides_FieldSet_CannotBeEmpty(MemberInfo member)
    {
        return new SchemaException(
            SchemaErrorBuilder.New()
                .SetMessage(ThrowHelper_Provides_FieldSet_CannotBeEmpty)
                .SetExtension(nameof(member), member)
                .Build());
    }

    public static SchemaException Requires_Field_CannotBeEmpty(MemberInfo member)
    {
        return new SchemaException(
            SchemaErrorBuilder.New()
                .SetMessage(ThrowHelper_Requires_Field_CannotBeEmpty)
                .SetExtension(nameof(member), member)
                .Build());
    }

    public static SchemaException Requires_FieldSet_CannotBeEmpty(MemberInfo member)
    {
        return new SchemaException(
            SchemaErrorBuilder.New()
                .SetMessage(ThrowHelper_Requires_FieldSet_CannotBeEmpty)
                .SetExtension(nameof(member), member)
                .Build());
    }

    public static GraphQLException Entities_Representation_Typename_Missing()
    {
        return new GraphQLException(
            ErrorBuilder.New()
                .SetMessage(ThrowHelper_Entities_Representation_Typename_Missing)
                .Build());
    }

    public static GraphQLException Entities_Representation_Typename_Invalid(object? value)
    {
        return new GraphQLException(
            ErrorBuilder.New()
                .SetMessage(ThrowHelper_Entities_Representation_Typename_Invalid, value ?? "null")
                .Build());
    }

    public static GraphQLException Entities_Representation_Entity_NotFound(string name)
    {
        return new GraphQLException(
            ErrorBuilder.New()
                .SetMessage(ThrowHelper_Entities_Representation_Entity_NotFound, name)
                .Build());
    }

    public static GraphQLException Entities_Representation_Resolver_Error(string typeName, Exception exception)
    {
        throw new GraphQLException(
            ErrorBuilder.New()
                .SetMessage(ThrowHelper_Entities_Representation_Resolver_Error, typeName, exception.Message)
                .SetException(exception)
                .Build());
    }

    public static SchemaException Directive_Field_NotFound(string directive, string field)
    {
        return new SchemaException(
            SchemaErrorBuilder.New()
                .SetMessage(
                    ThrowHelper_Directive_Field_NotFound,
                    directive,
                    field)
                .Build());
    }
}