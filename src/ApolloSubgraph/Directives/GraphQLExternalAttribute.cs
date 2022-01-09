using System;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloSubgraph;

/// <summary>
/// The <c>@external</c> directive is used to mark a field as owned by another service.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface |
                AttributeTargets.Property, AllowMultiple = true)]
public sealed class GraphQLExternalAttribute : DescriptorAttribute
{
    private readonly string? _field;

    /// <summary>
    /// Initializes a new instance of <see cref="GraphQLExternalAttribute"/>.
    /// </summary>
    /// <param name="field">
    /// The field that is external (can be <see langword="null"/> if the attribute is used on a field).
    /// </param>
    public GraphQLExternalAttribute(string? field = null)
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
                fieldDescriptor.External();
                break;
        }
    }

    private void Configure(IObjectTypeDescriptor descriptor, Type type)
    {
        if (string.IsNullOrWhiteSpace(_field))
        {
            throw ThrowHelper.External_FieldSet_CannotBeEmpty(type);
        }

        const string key = ExternalDirectiveType.Names.InterceptorKey;
        descriptor.AppendContextData(key, new DirectiveTypeInterceptorField(_field!));
    }
}