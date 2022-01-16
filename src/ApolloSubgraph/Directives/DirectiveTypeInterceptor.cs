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
        switch(definition)
        {
            case ObjectTypeDefinition x:
                TryAddObjectDirective(x);
                break;
            case InterfaceTypeDefinition x:
                TryAddInterfaceDirectives(x);
                break;
        }
    }

    public override void OnBeforeCompleteType(
        ITypeCompletionContext _,
        DefinitionBase? definition,
        IDictionary<string, object?> __)
    {
        switch(definition)
        {
            case ObjectTypeDefinition x:
                TryAddAnnotationDirectives(x);
                break;
            case InterfaceTypeDefinition x:
                TryAddAnnotationDirectives(x);
                break;
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

    private static void TryAddAnnotationDirectives(InterfaceTypeDefinition definition)
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

    private static void TryAddInterfaceDirectives(InterfaceTypeDefinition definition)
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
        if (!TryGetInterceptorFields(definition, key, out var fields))
        {
            return;
        }
        foreach (var field in fields!)
        {
            AddFieldDirective(
                definition.Fields.FirstOrDefault(x => x.Name == field.Name),
                definition,
                createDirective,
                field);
        }
    }

    private static void TryAddFieldDirectives(
        InterfaceTypeDefinition definition,
        string key,
        Func<DirectiveTypeInterceptorField, DirectiveDefinition> createDirective)
    {
        if (!TryGetInterceptorFields(definition, key, out var fields))
        {
            return;
        }
        foreach (var field in fields!)
        {
            AddFieldDirective(
                definition.Fields.FirstOrDefault(x => x.Name == field.Name),
                definition,
                createDirective,
                field);
        }
    }

    private static bool TryGetInterceptorFields(
        IDefinition definition,
        string key,
        out List<DirectiveTypeInterceptorField>? fields)
    {
        return definition.ContextData.TryGetValue(key, out fields);
    }

    private static void AddFieldDirective(
        IHasDirectiveDefinition? target,
        IDefinition definition,
        Func<DirectiveTypeInterceptorField, DirectiveDefinition> createDirective,
        DirectiveTypeInterceptorField field)
    {
        if (target == null)
        {
            ThrowHelper.Directive_Field_NotFound(definition.Name, field.Name);
        }
        var directive = createDirective(field);
        target?.Directives.Add(directive);
    }
}