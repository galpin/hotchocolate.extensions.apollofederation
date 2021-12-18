using System.Collections.Generic;
using HotChocolate.Configuration;
using HotChocolate.Language;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class GraphQLKeyAttributeInterceptor : TypeInterceptor
{
    public override void OnAfterInitialize(
        ITypeDiscoveryContext context,
        DefinitionBase? definition,
        IDictionary<string, object?> contextData)
    {
        if (definition is ObjectTypeDefinition objectDefinition)
        {
            TryAddFieldKeysToObject(objectDefinition);
        }
    }

    private static void TryAddFieldKeysToObject(ObjectTypeDefinition objectDefinition)
    {
        foreach (var field in objectDefinition.Fields)
        {
            if (field.ContextData.Remove(GraphQLKeyAttribute.Names.Marker))
            {
                objectDefinition.Directives.Add(CreateKeyDirective(field.Name));
            }
        }
    }

    private static DirectiveDefinition CreateKeyDirective(string fieldSet)
    {
        return new DirectiveDefinition(
            new DirectiveNode(
                KeyDirectiveType.Names.Key,
                new ArgumentNode(KeyDirectiveType.Names.Fields, fieldSet)));
    }
}