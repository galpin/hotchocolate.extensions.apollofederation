using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>@external</c> directive is used to mark a field as owned by another service.
/// </summary>
public sealed class GraphQLExternalAttribute : ObjectFieldDescriptorAttribute
{
    /// <inheritdoc />
    public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.External();
    }
}