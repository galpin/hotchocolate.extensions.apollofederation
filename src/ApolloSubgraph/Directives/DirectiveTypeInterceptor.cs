using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloSubgraph;

internal sealed class DirectiveTypeInterceptor : TypeInterceptor
{
    public override void OnAfterInitialize(
        ITypeDiscoveryContext _,
        DefinitionBase? definition,
        IDictionary<string, object?> __)
    {
        if (definition is ObjectTypeDefinition x)
        {
            TryAddObjectDirective(x);
        }
    }

    public override void OnBeforeCompleteType(
        ITypeCompletionContext _,
        DefinitionBase? definition,
        IDictionary<string, object?> __)
    {
        if (definition is ObjectTypeDefinition x)
        {
            TryAddAnnotationDirectives(x);
        }
    }

    private static void TryAddAnnotationDirectives(ObjectTypeDefinition definition)
    {
        TryAddFieldDirectives(
            definition,
            ExternalDirectiveType.Names.InterceptorKey,
            _ => ExternalDirectiveType.CreateDefinition());
        TryAddFieldDirectives(
            definition,
            ProvidesDirectiveType.Names.InterceptorKey,
            x => ProvidesDirectiveType.CreateDefinition(x.FieldSet!));
        TryAddFieldDirectives(
            definition,
            RequiresDirectiveType.Names.InterceptorKey,
            x => RequiresDirectiveType.CreateDefinition(x.FieldSet!));
    }

    private static void TryAddObjectDirective(ObjectTypeDefinition definition)
    {
        foreach (var field in definition.Fields)
        {
            if (field.ContextData.ContainsKey(KeyDirectiveType.Names.InterceptorKey))
            {
                definition.Directives.Add(KeyDirectiveType.CreateDefinition(field.Name));
            }
        }
    }

    private static void TryAddFieldDirectives(
        ObjectTypeDefinition definition,
        string key,
        Func<DirectiveTypeInterceptorField, DirectiveDefinition> createDirective)
    {
        if (!definition.ContextData.TryGetValue<List<DirectiveTypeInterceptorField>>(key, out var fields))
        {
            return;
        }
        foreach (var field in fields!)
        {
            var target = definition.Fields.FirstOrDefault(x => x.Name == field.Name);
            if (target == null)
            {
                ThrowHelper.Directive_Field_NotFound(definition.Name, field.Name);
            }
            var directive = createDirective(field);
            target?.Directives.Add(directive);
        }
    }
}