using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using static HotChocolate.Extensions.ApolloFederation.ThrowHelper;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>@provides</c> directive is used to annotate the expected returned field set from a field on a base type
/// that is guaranteed to be selectable by the gateway.
/// </summary>
public sealed class GraphQLProvidesAttribute : ObjectFieldDescriptorAttribute
{
    private readonly string _fieldSet;

    /// <summary>
    /// Initializes a new instance of <see cref="GraphQLProvidesAttribute"/>.
    /// </summary>
    /// <param name="fieldSet">The field set that is guaranteed to be selectable by the gateway.</param>
    public GraphQLProvidesAttribute(string fieldSet)
    {
        _fieldSet = fieldSet;
    }

    /// <inheritdoc />
    public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        if (string.IsNullOrWhiteSpace(_fieldSet))
        {
            throw Provides_FieldSet_CannotBeEmpty(member);
        }
        descriptor.Provides(_fieldSet);
    }
}