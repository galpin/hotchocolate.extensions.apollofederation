using System.Collections.Generic;
using System.Linq;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class EntityTypeInterceptor : TypeInterceptor
{
    private readonly HashSet<ObjectType> _entityTypes = new();

    public override void OnAfterInitialize(
        ITypeDiscoveryContext context,
        DefinitionBase? definition,
        IDictionary<string, object?> _)
    {
        if (context.Type is ObjectType objectType && definition is ObjectTypeDefinition objectTypeDefinition)
        {
            if (objectTypeDefinition.Directives.Any(IsKeyDirective))
            {
                _entityTypes.Add(objectType);
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
            foreach (var entityType in _entityTypes.OrderBy(x => x.Name))
            {
                unionTypeDefinition.Types.Add(TypeReference.Create(entityType));
            }
        }
    }
}