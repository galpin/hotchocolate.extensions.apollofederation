using System.Collections.Generic;
using HotChocolate.Configuration;
using HotChocolate.Language;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class KeyDirectiveTypeInterceptor : TypeInterceptor
{
    public override void OnAfterInitialize(
        ITypeDiscoveryContext _,
        DefinitionBase? definition,
        IDictionary<string, object?> __)
    {
        switch(definition)
        {
            case ObjectTypeDefinition x:
                TryAddFieldKeysToObject(x);
                break;
            case InterfaceTypeDefinition x:
                TryAddFieldKeysToInterface(x);
                break;
        }
    }

    private static void TryAddFieldKeysToObject(ObjectTypeDefinition objectDefinition)
    {
        foreach (var field in objectDefinition.Fields)
        {
            if (field.ContextData.ContainsKey(KeyDirectiveType.Names.InterceptorKey))
            {
                objectDefinition.Directives.Add(CreateKeyDirective(field.Name));
            }
        }
    }

    private static void TryAddFieldKeysToInterface(InterfaceTypeDefinition interfaceDefinition)
    {
        foreach (var field in interfaceDefinition.Fields)
        {
            if (field.ContextData.ContainsKey(KeyDirectiveType.Names.InterceptorKey))
            {
                interfaceDefinition.Directives.Add(CreateKeyDirective(field.Name));
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