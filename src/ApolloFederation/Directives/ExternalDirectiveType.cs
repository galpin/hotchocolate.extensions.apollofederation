using HotChocolate.Types;

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

    internal static class Names
    {
        public const string External = "external";
    }
}