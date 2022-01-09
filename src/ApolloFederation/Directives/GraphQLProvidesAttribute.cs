using System;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>@provides</c> directive is used to annotate the expected returned field set from a field on a base type
/// that is guaranteed to be selectable by the gateway.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface |
                AttributeTargets.Property, AllowMultiple = true)]
public sealed class GraphQLProvidesAttribute : DescriptorAttribute
{
    private readonly string? _field;
    private readonly string _fieldSet;

    /// <summary>
    /// Initializes a new instance of <see cref="GraphQLProvidesAttribute"/>.
    /// </summary>
    /// <param name="fieldSet">The field set that is guaranteed to be selectable by the gateway.</param>
    public GraphQLProvidesAttribute(string fieldSet)
    {
        _fieldSet = fieldSet;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="GraphQLProvidesAttribute"/>.
    /// </summary>
    /// <param name="field">The field on which to set the directive.</param>
    /// <param name="fieldSet">The field set that is guaranteed to be selectable by the gateway.</param>
    public GraphQLProvidesAttribute(string field, string fieldSet)
        : this(fieldSet)
    {
        _field = field;
    }

    /// <inheritdoc />
    protected override void TryConfigure(IDescriptorContext _, IDescriptor descriptor, ICustomAttributeProvider element)
    {
        switch (descriptor)
        {
            case IObjectTypeDescriptor objectDescriptor when element is Type type:
                Configure(objectDescriptor, type);
                break;
            case IObjectFieldDescriptor fieldDescriptor when element is MemberInfo:
                fieldDescriptor.Provides(_fieldSet);
                break;
        }
    }

    private void Configure(IObjectTypeDescriptor descriptor, Type type)
    {
        if (string.IsNullOrWhiteSpace(_field))
        {
            throw ThrowHelper.Provides_Field_CannotBeEmpty(type);
        }
        if (string.IsNullOrWhiteSpace(_fieldSet))
        {
            throw ThrowHelper.Provides_FieldSet_CannotBeEmpty(type);
        }

        const string key = ProvidesDirectiveType.Names.InterceptorKey;
        descriptor.AppendContextData(key, new DirectiveTypeInterceptorField(_field!, _fieldSet));
    }
}