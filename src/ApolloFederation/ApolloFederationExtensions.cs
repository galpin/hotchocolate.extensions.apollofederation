using System;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Provides extensions to <see cref="IRequestExecutorBuilder"/>.
/// </summary>
public static class ApolloFederationExtensions
{
    /// <summary>
    /// Adds support for Apollo Federation to the schema.
    /// </summary>
    /// <param name="builder">The <see cref="IRequestExecutorBuilder"/>.</param>
    /// <returns>The <see cref="IRequestExecutorBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="builder"/> is <see langword="null"/>.
    /// </exception>
    public static IRequestExecutorBuilder AddApolloFederation(this IRequestExecutorBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.ConfigureSchemaServices(x =>
        {
            x.TryAddSingleton<IEntityResolverRegistry, EntityResolverRegistry>();
            x.TryAddSingleton<IFederatedSchemaPrinter, FederatedSchemaPrinter>();
        });
        builder.ConfigureSchema(x =>
        {
            x.AddType<AnyType>();
            x.AddType<EntityType>();
            x.AddType<FieldSetType>();
            x.AddType<ServiceType>();
            x.AddType<ExtendsDirectiveType>();
            x.AddType<ExternalDirectiveType>();
            x.AddType<KeyDirectiveType>();
            x.AddType<ProvidesDirectiveType>();
            x.AddType<QueryTypeExtension>();
            x.AddType<RequiresDirectiveType>();
            // TypeInterceptor order is significant, do not re-order!
            x.TryAddTypeInterceptor<GraphQLKeyAttributeInterceptor>();
            x.TryAddTypeInterceptor<EntityResolverObjectInterceptor>();
            x.TryAddTypeInterceptor<EntityTypeInterceptor>();
            x.TryAddTypeInterceptor<EntityResolverMethodInterceptor>();
            x.TryAddSchemaInterceptor<EntityResolverSchemaInterceptor>();
        });
        return builder;
    }
}