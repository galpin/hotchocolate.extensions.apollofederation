using System;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>_Service</c> type is used to expose the service's schema.
/// </summary>
public sealed class ServiceType : ObjectType
{
    private readonly IFederatedSchemaPrinter _federatedSchemaPrinter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceType"/> class.
    /// </summary>
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