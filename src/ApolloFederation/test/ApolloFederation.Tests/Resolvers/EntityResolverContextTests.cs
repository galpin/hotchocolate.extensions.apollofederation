using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class EntityResolverContextTests
{
    [Fact]
    public void Ctor_correctly_initializes_members()
    {
        var expectedServices = BuildServiceProvider();
        var expectedRepresentation = CreateRepresentation(("upc", "1"));

        var sut = CreateSut(expectedServices, expectedRepresentation);

        Assert.Same(expectedServices, sut.Services);
        Assert.Same(expectedRepresentation, sut.Representation);
    }

    [Fact]
     public void Services_resolves_bound_service()
     {
         var expected = new MyService();
         var services = BuildServiceProvider(x => x.AddSingleton(expected));

         var sut = CreateSut(services);
         var actual = sut.Services.GetService<MyService>();

         Assert.Same(expected, actual);
     }

     [Fact]
     public void Service_delegates_to_services()
     {
         var services = BuildServiceProvider(x => x.AddSingleton<MyService>());
         var sut = CreateSut(services);

         var expected = sut.Services.GetService<MyService>();
         var actual = sut.Service<MyService>();

         Assert.Same(expected, actual);
     }

     [Fact]
     public void Service_delegates_to_required_service()
     {
         var sut = CreateSut();

         Assert.Throws<InvalidOperationException>(() => sut.Service<MyService>());
     }

    private static EntityResolverContext CreateSut(
        IServiceProvider? services = null,
        IReadOnlyDictionary<string, object?>? representation = null)
    {
        services ??= BuildServiceProvider();
        representation ??= CreateRepresentation();
        return new EntityResolverContext(services, representation);
    }

    private sealed class MyService
    {
    }
}