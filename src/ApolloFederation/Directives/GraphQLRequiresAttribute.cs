using System;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>@requires</c> directive is used to annotate the required input field set from a base type for a resolver.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface |
                AttributeTargets.Property, AllowMultiple = true)]
public sealed class GraphQLRequiresAttribute : DescriptorAttribute
{
    private readonly string? _field;
    private readonly string _fieldSet;

    /// <summary>
    /// Initializes a new instance of <see cref="GraphQLRequiresAttribute"/>.
    /// </summary>
    /// <param name="fieldSet">The field set that is required from a base type for a resolver.</param>
    public GraphQLRequiresAttribute(string fieldSet)
    {
        _fieldSet = fieldSet;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="GraphQLRequiresAttribute"/>.
    /// </summary>
    /// <param name="field">The field on which to set the directive.</param>
    /// <param name="fieldSet">The field set that is guaranteed to be selectable by the gateway.</param>
    public GraphQLRequiresAttribute(string field, string fieldSet)
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
                fieldDescriptor.Requires(_fieldSet);
                break;
        }
    }

    private void Configure(IObjectTypeDescriptor descriptor, Type type)
    {
        if (string.IsNullOrWhiteSpace(_field))
        {
            throw ThrowHelper.Requires_Field_CannotBeEmpty(type);
        }
        if (string.IsNullOrWhiteSpace(_fieldSet))
        {
            throw ThrowHelper.Requires_FieldSet_CannotBeEmpty(type);
        }

        const string key = RequiresDirectiveType.Names.InterceptorKey;
        descriptor.AppendContextData(key, new DirectiveTypeInterceptorField(_field!, _fieldSet));
    }
}