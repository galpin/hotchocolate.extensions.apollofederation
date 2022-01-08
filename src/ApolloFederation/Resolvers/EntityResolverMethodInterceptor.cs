using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class EntityResolverMethodInterceptor : TypeInterceptor
{
    private readonly IEntityResolverRegistry _entityResolverRegistry;
    private readonly ObjectExtensionsMap _objectExtensions = new();
    private EntityType? _entityType;

    private static readonly HashSet<string> s_methodNames = new()
    {
        "ResolveEntity",
        "ResolveEntityAsync"
    };

    public EntityResolverMethodInterceptor(IEntityResolverRegistry entityResolverRegistry)
    {
        if (entityResolverRegistry is null)
        {
            throw new ArgumentNullException(nameof(entityResolverRegistry));
        }

        _entityResolverRegistry = entityResolverRegistry;
    }

    public override bool TriggerAggregations => true;

    public override void OnAfterInitialize(
        ITypeDiscoveryContext context,
        DefinitionBase? definition,
        IDictionary<string, object?> __)
    {
        if (context.Type is ObjectTypeExtension &&
            definition is ObjectTypeDefinition objectTypeDefinition &&
            objectTypeDefinition.FieldBindingType != null)
        {
            var extendsType = context.GetTypeName(objectTypeDefinition);
            _objectExtensions.Add(extendsType, objectTypeDefinition.FieldBindingType);
        }
    }

    public override void OnAfterCompleteType(
        ITypeCompletionContext context,
        DefinitionBase? definition,
        IDictionary<string, object?> __)
    {
        if (context.Type is EntityType entityType)
        {
            _entityType = entityType;
        }
    }

    public override void OnAfterCompleteTypes()
    {
        if (_entityType == null)
        {
            return;
        }
        foreach (var objectType in _entityType.Types.Values)
        {
            if (!TryAddResolverFromObject(objectType))
            {
                TryAddResolverFromObjectExtensions(objectType);
            }
        }
    }

    private bool TryAddResolverFromObject(ObjectType objectType)
    {
        return TryAddResolver(objectType, objectType.RuntimeType);
    }

    private void TryAddResolverFromObjectExtensions(ObjectType objectType)
    {
        foreach (var extensionType in _objectExtensions.Get(objectType.Name))
        {
            if (TryAddResolver(objectType, extensionType))
            {
                return;
            }
        }
    }

    private bool TryAddResolver(ObjectType objectType, Type containingType)
    {
        var resolver = TryCreateResolver(objectType.RuntimeType, containingType);
        if (resolver == null)
        {
            return false;
        }
        _entityResolverRegistry.Add(objectType.Name, resolver);
        return true;
    }

    private static EntityResolverDelegate? TryCreateResolver(Type entityType, Type containingType)
    {
        var method = GetPublicStaticMethods(containingType).FirstOrDefault(IsResolveEntityMethod);
        return method != null ? EntityResolverDelegateFactory.Compile(method) : null;

        bool IsResolveEntityMethod(MethodInfo candidate)
        {
            if (!s_methodNames.Contains(candidate.Name) && !HasEntityResolverAttribute(candidate))
            {
                return false;
            }
            var returnType = candidate.ReturnType;
            if (returnType == entityType)
            {
                return true;
            }
            if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
            {
                return false;
            }
            var genericTypeArguments = returnType.GetGenericArguments();
            if (genericTypeArguments.Length != 1)
            {
                return false;
            }
            return genericTypeArguments[0] == entityType;
        }
    }

    private static IEnumerable<MethodInfo> GetPublicStaticMethods(Type entityType)
    {
        return entityType.GetMethods(BindingFlags.Public | BindingFlags.Static);
    }

    private static bool HasEntityResolverAttribute(MethodInfo candidate)
    {
        return candidate.GetCustomAttribute<GraphQLEntityResolverAttribute>() != null;
    }

    private sealed class ObjectExtensionsMap
    {
        private readonly Dictionary<NameString, List<Type>> _extensions = new();

        public void Add(NameString extends, Type bindingType)
        {
            if (!_extensions.TryGetValue(extends, out var types))
            {
                _extensions.Add(extends, new List<Type> { bindingType });
            }
            else
            {
                types.Add(bindingType);
            }
        }

        public IEnumerable<Type> Get(NameString extends)
        {
            return _extensions.TryGetValue(extends, out var types) ? types : Enumerable.Empty<Type>();
        }
    }
}