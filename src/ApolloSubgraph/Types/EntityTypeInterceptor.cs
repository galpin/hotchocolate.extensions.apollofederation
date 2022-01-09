using System.Collections.Generic;
using System.Linq;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloSubgraph;

internal sealed class EntityTypeInterceptor : TypeInterceptor
{
    private readonly HashSet<NameString> _entityTypes = new();

    public override void OnAfterInitialize(
        ITypeDiscoveryContext context,
        DefinitionBase? definition,
        IDictionary<string, object?> _)
    {
        switch (context.Type)
        {
            case ObjectType:
            case ObjectTypeExtension:
            {
                if (definition is ObjectTypeDefinition objectTypeDefinition &&
                    objectTypeDefinition.Directives.Any(IsKeyDirective))
                {
                    _entityTypes.Add(context.GetTypeName(objectTypeDefinition));
                }
                break;
            }
        }

        bool IsKeyDirective(DirectiveDefinition directive)
        {
            return directive.Reference is NameDirectiveReference { Name.Value: KeyDirectiveType.Names.Key };
        }
    }

    public override void OnBeforeCompleteType(
        ITypeCompletionContext context,
        DefinitionBase? definition,
        IDictionary<string, object?> _)
    {
        if (context.Type is EntityType && definition is UnionTypeDefinition unionTypeDefinition)
        {
            foreach (var entityType in _entityTypes.OrderBy(x => x))
            {
                unionTypeDefinition.Types.Add(TypeReference.Create(entityType));
            }
        }
    }
}