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

    public override void OnAfterCompleteType(
        ITypeCompletionContext context,
        DefinitionBase? _,
        IDictionary<string, object?> __)
    {
        if (context.Type is not EntityType entityType)
        {
            return;
        }
        foreach (var objectType in entityType.Types.Values)
        {
            TryRegisterResolver(objectType);
        }
    }

    private void TryRegisterResolver(ObjectType objectType)
    {
        var resolver = TryCreateResolverDelegate(objectType.RuntimeType);
        if (resolver != null)
        {
            _entityResolverRegistry.Add(objectType.Name, resolver);
        }
    }

    private static EntityResolverDelegate? TryCreateResolverDelegate(Type entityType)
    {
        var method = GetPublicStaticMethods(entityType).FirstOrDefault(IsResolveEntityMethod);
        return method != null ? EntityResolverDelegateFactory.Compile(method) : null;

        bool IsResolveEntityMethod(MethodInfo candidate)
        {
            if (!s_methodNames.Contains(candidate.Name))
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
}