using System;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloSubgraph;

/// <summary>
/// The <c>_Service</c> type is used to expose the service's schema.
/// </summary>
public sealed class ServiceType : ObjectType
{
    private readonly IFederatedSchemaPrinter _federatedSchemaPrinter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceType"/> class.
    /// </summary>
    /// <param name="federatedSchemaPrinter">The schema printer.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="federatedSchemaPrinter"/> is <see langword="null"/>.
    /// </exception>
    public ServiceType(IFederatedSchemaPrinter federatedSchemaPrinter)
    {
        if (federatedSchemaPrinter is null)
        {
            throw new ArgumentNullException(nameof(federatedSchemaPrinter));
        }

        _federatedSchemaPrinter = federatedSchemaPrinter;
    }

    /// <inheritdoc />
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor.Name(Names.Service);

        descriptor.Field(Names.Sdl)
            .Type<NonNullType<StringType>>()
            .Resolve(x => _federatedSchemaPrinter.Print(x.Schema));
    }

    internal static class Names
    {
        public const string Service = "_Service";
        public const string Sdl = "sdl";
    }
}
