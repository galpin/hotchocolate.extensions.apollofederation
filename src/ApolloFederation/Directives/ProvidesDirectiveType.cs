using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;
using DirectiveLocation = HotChocolate.Types.DirectiveLocation;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>@provides</c> directive is used to annotate the expected returned field set from a field on a base type
/// that is guaranteed to be selectable by the gateway.
/// </summary>
public sealed class ProvidesDirectiveType : DirectiveType
{
    /// <inheritdoc />
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        descriptor.Name(Names.Provides)
            .Location(DirectiveLocation.FieldDefinition);

        descriptor.Argument(Names.Fields)
            .Type<NonNullType<FieldSetType>>();
    }

    internal static DirectiveDefinition CreateDefinition(string fieldSet)
    {
        return new DirectiveDefinition(
            new DirectiveNode(
                Names.Provides,
                new ArgumentNode(Names.Fields, fieldSet)));
    }

    internal static class Names
    {
        public const string Provides = "provides";
        public const string Fields = "fields";
        public const string InterceptorKey = "ApolloFederation.ProvidesDirective.InterceptorKey";
    }
}