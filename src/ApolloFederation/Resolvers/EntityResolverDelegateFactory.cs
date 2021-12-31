using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace HotChocolate.Extensions.ApolloFederation;

internal static class EntityResolverDelegateFactory
{
    public static EntityResolverDelegate Create<TReturn>(Func<IEntityResolverContext, TReturn?> resolver)
    {
        return x => WrapResultHelper(resolver(x));
    }

    public static EntityResolverDelegate Create<TReturn>(Func<IEntityResolverContext, Task<TReturn?>> resolver)
    {
        return async x => await AwaitTaskHelper(resolver(x)).ConfigureAwait(false);
    }

    public static EntityResolverDelegate Compile(MethodInfo method)
    {
        var context = Expression.Parameter(typeof(IEntityResolverContext), "context");
        var genericResolver = Expression.Call(method, context);
        var resolver = EnsureResolveResult(genericResolver, method.ReturnType);
        return Expression.Lambda<EntityResolverDelegate>(resolver, context).Compile();
    }

    private static Expression EnsureResolveResult(Expression resolver, Type result)
    {
        if (typeof(Task).IsAssignableFrom((Type?)result) && result.IsGenericType)
        {
            return AwaitTaskMethodCall(resolver, result.GetGenericArguments()[0]);
        }
        return WrapResult(resolver, result);
    }

    private static MethodCallExpression WrapResult(Expression expression, Type value)
    {
        var wrapResultHelper = s_wrapResultHelper.MakeGenericMethod(value);
        return Expression.Call(wrapResultHelper, expression);
    }

    private static MethodCallExpression AwaitTaskMethodCall(Expression taskExpression, Type value)
    {
        var awaitHelper = s_awaitTaskHelper.MakeGenericMethod(value);
        return Expression.Call(awaitHelper, taskExpression);
    }

    private static async ValueTask<object?> AwaitTaskHelper<T>(Task<T>? task)
    {
        return task is null ? null : await task.ConfigureAwait(false);
    }

    private static ValueTask<object?> WrapResultHelper<T>(T result)
    {
        return new ValueTask<object?>(result);
    }

    private static readonly MethodInfo s_awaitTaskHelper = GetPrivateStaticMethod(nameof(AwaitTaskHelper));
    private static readonly MethodInfo s_wrapResultHelper = GetPrivateStaticMethod(nameof(WrapResultHelper));

    private static MethodInfo GetPrivateStaticMethod(string name)
    {
        return typeof(EntityResolverDelegateFactory).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)!;
    }
}