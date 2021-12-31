using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>@requires</c> directive is used to annotate the required input field set from a base type for a resolver.
/// </summary>
public sealed class GraphQLRequiresAttribute : ObjectFieldDescriptorAttribute
{
    private readonly string _fieldSet;

    /// <summary>
    /// Initializes a new instance of <see cref="GraphQLRequiresAttribute"/>.
    /// </summary>
    /// <param name="fieldSet">The fields that is required from a base type for a resolver.</param>
    public GraphQLRequiresAttribute(string fieldSet)
    {
        _fieldSet = fieldSet;
    }

    /// <inheritdoc />
    public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        if (string.IsNullOrWhiteSpace(_fieldSet))
        {
            throw ThrowHelper.Requires_FieldSet_CannotBeEmpty(member);
        }
        descriptor.Requires(_fieldSet);
    }
}