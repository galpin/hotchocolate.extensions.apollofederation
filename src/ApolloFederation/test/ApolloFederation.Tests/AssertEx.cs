using System;
using System.Linq;
using HotChocolate.Language;
using HotChocolate.Types;
using Xunit;

namespace HotChocolate.Extensions.ApolloFederation;

internal static class AssertEx
{
    public static void Directive(
        IDirective actual,
        NameString expectedName,
        params (string Name, string Value)[] expectedArguments)
    {
        var node = actual.ToNode();
        var argumentInspectors = expectedArguments.Select(expected =>
            new Action<ArgumentNode>(x => Argument(expected, x))).ToArray();
        Assert.Equal(expectedName, actual.Name);
        Assert.Collection(node.Arguments, argumentInspectors);
    }

    private static void Argument((string Name, string Value) expected, ArgumentNode actual)
    {
        Assert.Equal(expected.Name, actual.Name.ToString());
        Assert.Equal(expected.Value, actual.Value.ToString());
    }

    public static void Sdl(string expected, string actual)
    {
        Assert.Equal(Normalize(expected), Normalize(actual));

        static string Normalize(string expected)
        {
            return Utf8GraphQLParser.Parse(expected).ToString();
        }
    }
}