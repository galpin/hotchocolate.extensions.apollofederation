using System;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The root query extensions are used for schema introspection and for resolving entities from external services.
/// </summary>
public sealed class QueryTypeExtension : ObjectTypeExtension
{
    private readonly QueryTypeResolvers _queryTypeResolvers;

    internal QueryTypeExtension(QueryTypeResolvers queryTypeResolvers)
    {
        if (queryTypeResolvers is null)
        {
            throw new ArgumentNullException(nameof(queryTypeResolvers));
        }

        _queryTypeResolvers = queryTypeResolvers;
    }

    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor.Name(Names.Query);

        descriptor.Field(Names.Entities)
            .Type<NonNullType<ListType<EntityType>>>()
            .Argument(
                Names.Representations,
                x => x.Type<NonNullType<ListType<NonNullType<AnyType>>>>())
            .Resolve(x => _queryTypeResolvers.GetEntitiesAsync(x));

        descriptor.Field(Names.Service)
            .Type<ServiceType>()
            .Resolve(new object());
    }

    internal static class Names
    {
        public const string Query = "Query";
        public const string Service = "_service";
        public const string Entities = "_entities";
        public const string Representations = "representations";
    }
}