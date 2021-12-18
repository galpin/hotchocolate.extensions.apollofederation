using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>@extends</c> directive is used to indicate a type extension.
/// </summary>
public sealed class ExtendsDirectiveType : DirectiveType
{
    /// <inheritdoc />
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        descriptor.Name(Names.Extends)
            .Location(DirectiveLocation.Object | DirectiveLocation.Interface);
    }

    internal static class Names
    {
        public const string Extends = "extends";
    }
}