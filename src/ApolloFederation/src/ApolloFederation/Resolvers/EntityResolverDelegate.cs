using System.Threading.Tasks;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The delegate which describes the interface for resolving a federated entity.
/// </summary>
/// <param name="context">The resolver context.</param>
/// <returns>
/// A task that provides the resolved entity or <see langword="null"/> if the entity cannot be found.
/// </returns>
public delegate ValueTask<object?> EntityResolverDelegate(IEntityResolverContext context);
