using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;
using DirectiveLocation = HotChocolate.Types.DirectiveLocation;

namespace HotChocolate.Extensions.ApolloSubgraph;

/// <summary>
/// The <c>@requires</c> directive is used to annotate the required input field set from a base type for a resolver.
/// </summary>
public sealed class RequiresDirectiveType : DirectiveType
{
    /// <inheritdoc />
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        descriptor.Name(Names.Requires)
            .Location(DirectiveLocation.FieldDefinition);

        descriptor.Argument(Names.Fields)
            .Type<NonNullType<FieldSetType>>();
    }

    internal static DirectiveDefinition CreateDefinition(string fieldSet)
    {
        return new DirectiveDefinition(
            new DirectiveNode(
                Names.Requires,
                new ArgumentNode(Names.Fields, fieldSet)));
    }

    internal static class Names
    {
        public const string Requires = "requires";
        public const string Fields = "fields";
        public const string InterceptorKey = "ApolloSubgraph.RequiresDirective.InterceptorKey";
    }
}