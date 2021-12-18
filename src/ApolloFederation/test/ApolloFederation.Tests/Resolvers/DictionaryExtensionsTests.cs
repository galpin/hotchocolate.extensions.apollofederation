using System;
using System.Collections.Generic;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;

// ReSharper disable InvokeAsExtensionMethod

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

public class DictionaryExtensionsTests
{
    [Fact]
    public void GetValue_when_key_exists_returns_value()
    {
        var expected = "42";
        var source = CreateRepresentation(("id", expected));

        var actual = DictionaryExtensions.GetValue<string>(source, "id");

        Assert.IsType<string>(actual);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetValue_when_wrong_type_specified_throws()
    {
        var source = CreateRepresentation(("id", "42"));

        Assert.Throws<InvalidCastException>(() => DictionaryExtensions.GetValue<int>(source, "id"));
    }

    [Fact]
    public void GetValue_when_key_does_exist_throws()
    {
        var source = CreateRepresentation();

        Assert.Throws<KeyNotFoundException>(() => DictionaryExtensions.GetValue<string>(source, "missing"));
    }

    [Fact]
    public void GetValueOrDefault_when_key_exists_returns_value()
    {
        var expected = "42";
        var source = CreateRepresentation(("id", expected));

        var actual = DictionaryExtensions.GetValueOrDefault<string>(source, "id");

        Assert.IsType<string>(actual);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetValueOrDefault_when_key_does_not_exist_returns_default()
    {
        var source = CreateRepresentation();

        var actual = DictionaryExtensions.GetValueOrDefault<string>(source, "id");

        Assert.Null(actual);
    }
}
