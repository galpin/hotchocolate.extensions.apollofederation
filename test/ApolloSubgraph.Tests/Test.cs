using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloSubgraph;

internal static class Test
{
    public static async Task<ISchema> BuildSchemaAsync(Action<IRequestExecutorBuilder> configureExecutor)
    {
        return (await BuildRequestExecutorAsync(configureExecutor)).Schema;
    }

    public static async Task<IRequestExecutor> BuildRequestExecutorAsync(
        Action<IRequestExecutorBuilder> configureExecutor)
    {
        var builder = new ServiceCollection()
            .AddGraphQL()
            .AddApolloSubgraph()
            .ConfigureSchema(x => x.UseFallback());
        configureExecutor(builder);
        return await builder.BuildRequestExecutorAsync();
    }

    public static async Task<ISchema> BuildSchemaAsync()
    {
        return await BuildSchemaAsync(builder =>
        {
            builder.AddDocumentFromString(@"
                type Empty @key(fields: ""id"") {
                    id: Int
                }
                type Query
            ");
        });
    }

    public static IServiceProvider BuildServiceProvider(Action<IServiceCollection>? configure = null)
    {
        var services = new ServiceCollection();
        configure?.Invoke(services);
        return services.BuildServiceProvider();
    }

    public static IReadOnlyDictionary<string, object?> CreateRepresentation(params (string Key, object? Value)[] items)
    {
        return items.ToDictionary(x => x.Key, x => x.Value);
    }

    private static ISchemaBuilder UseFallback(this ISchemaBuilder builder)
    {
        return builder.Use(next =>
        {
            return ctx =>
            {
                try
                {
                    return next(ctx);
                }
                catch (SchemaException exception)
                {
                    if (exception.Errors.Any(x => x.Code == ErrorCodes.Schema.NoResolver))
                    {
                        return default;
                    }
                    throw;
                }
            };
        });
    }
}