using System;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloSubgraph;

/// <summary>
/// The <c>@extends</c> directive is used to indicate a type extension.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = true)]
public sealed class GraphQLExtendsAttribute : ObjectTypeDescriptorAttribute
{
    /// <inheritdoc />
    public override void OnConfigure(IDescriptorContext context, IObjectTypeDescriptor descriptor, Type type)
    {
        descriptor.Extends();
    }
}