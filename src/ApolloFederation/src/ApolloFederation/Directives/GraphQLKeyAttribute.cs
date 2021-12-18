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
    /// <param name="fieldSet">The fields that describe the key (ignored if used on a property).</param>
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
                if (string.IsNullOrWhiteSpace(_fieldSet))
                {
                    throw Key_FieldSet_CannotBeEmpty(objectType);
                }
                objectDescriptor.Key(_fieldSet!);
                break;
            case IObjectFieldDescriptor fieldDescriptor when element is MemberInfo:
                fieldDescriptor.Extend().OnBeforeCreate(x => x.ContextData[Names.Marker] = true);
                break;
        }
    }

    internal static class Names
    {
        public const string Marker = "ApolloFederation.Markers.Key";
    }
}