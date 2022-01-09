using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;
using DirectiveLocation = HotChocolate.Types.DirectiveLocation;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>@external</c> directive is used to mark a field as owned by another service.
/// </summary>
public sealed class ExternalDirectiveType : DirectiveType
{
    /// <inheritdoc />
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        descriptor.Name(Names.External)
            .Location(DirectiveLocation.FieldDefinition);
    }

    internal static DirectiveDefinition CreateDefinition()
    {
        return new DirectiveDefinition(new DirectiveNode(Names.External));
    }

    internal static class Names
    {
        public const string External = "external";
        public const string InterceptorKey = "ApolloFederation.ExternalDirective.InterceptorKey";
    }
}