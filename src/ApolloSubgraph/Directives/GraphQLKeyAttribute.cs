using System;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloSubgraph;

/// <summary>
/// The <c>@key</c> indicates fields that can be used to uniquely identify and fetch an object or interface.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface |
                AttributeTargets.Property, AllowMultiple = true)]
public sealed class GraphQLKeyAttribute : DescriptorAttribute
{
    private readonly string? _fieldSet;

    /// <summary>
    /// Initializes a new instance of <see cref="GraphQLKeyAttribute"/>.
    /// </summary>
    /// <param name="fieldSet">
    /// The field set that describes the key (can be <see langword="null"/> if the attribute is used on a field).
    /// </param>
    public GraphQLKeyAttribute(string? fieldSet = null)
    {
        _fieldSet = fieldSet;
    }

    /// <inheritdoc />
    protected override void TryConfigure(IDescriptorContext _, IDescriptor descriptor, ICustomAttributeProvider element)
    {
        switch (descriptor)
        {
            case IObjectTypeDescriptor objectDescriptor when element is Type type:
                Configure(objectDescriptor, type);
                break;
            case IInterfaceTypeDescriptor interfaceDescriptor when element is Type type:
                Configure(interfaceDescriptor, type);
                break;
            case IObjectFieldDescriptor fieldDescriptor:
                fieldDescriptor.SetContextData(KeyDirectiveType.Names.InterceptorKey, true);
                break;
            case IInterfaceFieldDescriptor fieldDescriptor:
                fieldDescriptor.SetContextData(KeyDirectiveType.Names.InterceptorKey, true);
                break;
        }
    }

    private void Configure(IObjectTypeDescriptor descriptor, Type type)
    {
        if (string.IsNullOrWhiteSpace(_fieldSet))
        {
            throw ThrowHelper.Key_FieldSet_CannotBeEmpty(type);
        }

        descriptor.Key(_fieldSet!);
    }

    private void Configure(IInterfaceTypeDescriptor descriptor, Type type)
    {
        if (string.IsNullOrWhiteSpace(_fieldSet))
        {
            throw ThrowHelper.Key_FieldSet_CannotBeEmpty(type);
        }

        descriptor.Key(_fieldSet!);
    }
}