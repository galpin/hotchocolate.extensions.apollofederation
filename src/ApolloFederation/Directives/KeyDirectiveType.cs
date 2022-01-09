using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;
using DirectiveLocation = HotChocolate.Types.DirectiveLocation;

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

    internal static DirectiveDefinition CreateDefinition(string fieldSet)
    {
        return new DirectiveDefinition(
            new DirectiveNode(
                Names.Key,
                new ArgumentNode(Names.Fields, fieldSet)));
    }

    internal static class Names
    {
        public const string Key = "key";
        public const string Fields = "fields";
        public const string InterceptorKey = "ApolloFederation.KeyDirective.InterceptorKey";
    }
}