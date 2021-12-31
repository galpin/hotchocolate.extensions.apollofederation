using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>_Entity</c> type is used to provide the service's types that use the <c>@key</c> directive.
/// </summary>
public sealed class EntityType : UnionType
{
    /// <inheritdoc />
    protected override void Configure(IUnionTypeDescriptor descriptor)
    {
        descriptor.Name(Names.Entity);
    }

    internal static class Names
    {
        public const string Entity = "_Entity";
    }
}