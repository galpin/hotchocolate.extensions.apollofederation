using System;

namespace HotChocolate.Extensions.ApolloSubgraph;

/// <summary>
/// Specifies a mechanism for printing the federated service SDL.
/// </summary>
public interface IFederatedSchemaPrinter
{
    /// <summary>
    /// Prints the federated SDL including the service types and federation directives.
    /// </summary>
    /// <param name="schema">The schema to print.</param>
    /// <returns>A <see langword="string"/> containing the federated SDL.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="schema"/> is <see langword="null"/>.
    /// </exception>
    string Print(ISchema schema);
}