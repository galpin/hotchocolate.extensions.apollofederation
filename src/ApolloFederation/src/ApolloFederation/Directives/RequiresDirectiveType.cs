using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

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

    internal static class Names
    {
        public const string Requires = "requires";
        public const string Fields = "fields";
    }
}