using System;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloSubgraph;

/// <summary>
/// The <c>@extends</c> directive is used to indicate a type extension.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = true)]
public sealed class GraphQLExtendsAttribute : DescriptorAttribute
{
    /// <inheritdoc />
    protected override void TryConfigure(IDescriptorContext _, IDescriptor descriptor, ICustomAttributeProvider __)
    {
        switch (descriptor)
        {
            case IObjectTypeDescriptor objectDescriptor:
                objectDescriptor.Extends();
                break;
            case IInterfaceTypeDescriptor interfaceDescriptor:
                interfaceDescriptor.Extends();
                break;
        }
    }
}