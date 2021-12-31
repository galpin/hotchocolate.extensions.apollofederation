namespace HotChocolate.Extensions.ApolloFederation;

internal interface IEntityResolverRegistry
{
    void Add(NameString typeName, EntityResolverDelegate value);

    bool TryGet(NameString typeName, out EntityResolverDelegate? result);
}