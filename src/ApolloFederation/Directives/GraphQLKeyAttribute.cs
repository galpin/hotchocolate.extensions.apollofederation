using System;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using static HotChocolate.Extensions.ApolloFederation.ThrowHelper;

namespace HotChocolate.Extensions.ApolloFederation;

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
    /// The field set that describes the key (can be <see langword="null"/> if the attribute is used on a property).
    /// </param>
    public GraphQLKeyAttribute(string? fieldSet = null)
    {
        _fieldSet = fieldSet;
    }

    /// <inheritdoc />
    protected override void TryConfigure(IDescriptorContext _, IDescriptor descriptor,ICustomAttributeProvider element)
    {
        switch (descriptor)
        {
            case IObjectTypeDescriptor objectDescriptor when element is Type objectType:
                VerifyFieldSet(objectType);
                objectDescriptor.Key(_fieldSet!);
                break;
            case IInterfaceTypeDescriptor objectDescriptor when element is Type interfaceType:
                VerifyFieldSet(interfaceType);
                objectDescriptor.Key(_fieldSet!);
                break;
            case IObjectFieldDescriptor fieldDescriptor when element is MemberInfo:
                fieldDescriptor.SetContextData(KeyDirectiveType.Names.InterceptorKey, true);
                break;
            case IInterfaceFieldDescriptor fieldDescriptor when element is MemberInfo:
                fieldDescriptor.SetContextData(KeyDirectiveType.Names.InterceptorKey, true);
                break;
        }

        void VerifyFieldSet(Type type)
        {
            if (string.IsNullOrWhiteSpace(_fieldSet))
            {
                throw Key_FieldSet_CannotBeEmpty(type);
            }
        }
    }
}