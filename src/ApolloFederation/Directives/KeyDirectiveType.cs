using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>@key</c> indicates fields that can be used to uniquely identify and fetch an object or interface.
/// </summary>
public sealed class KeyDirectiveType : DirectiveType
{
    /// <inheritdoc />
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        descriptor.Name(Names.Key)
            .Location(DirectiveLocation.Object | DirectiveLocation.Interface)
            .Repeatable();

        descriptor.Argument(Names.Fields)
            .Type<NonNullType<FieldSetType>>();
    }

    internal static class Names
    {
        public const string Key = "key";
        public const string Fields = "fields";
    }
}