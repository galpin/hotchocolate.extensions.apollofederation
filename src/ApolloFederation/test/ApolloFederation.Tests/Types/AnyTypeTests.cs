using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static HotChocolate.Extensions.ApolloFederation.Test;
using static HotChocolate.Language.Utf8GraphQLParser;

namespace HotChocolate.Extensions.ApolloFederation.Types;

public class AnyTypeTests
{
    [Fact]
    public async Task Ctor_correctly_configures_object()
    {
        var schema = await BuildSchemaAsync();

        var sut = schema.GetType<AnyType>("_Any");

        Assert.Equal("_Any", sut.Name);
    }

    [Theory]
    [MemberData(nameof(Deserialize_supports_representations_provider))]
    public async Task Deserialize_supports_representations(
        string representation,
        IReadOnlyDictionary<string, object?> expected)
    {
        var schema = await BuildSchemaAsync();
        var sut = schema.GetType<AnyType>("_Any");
        var @object = Syntax.ParseObjectLiteral(representation);

        var actual = sut.Deserialize(@object);

        Assert.Equal(expected, actual);
    }

    private static IEnumerable<object?[]> Deserialize_supports_representations_provider()
    {
        yield return A(
            "{__typename: User}",
            CreateRepresentation(("__typename", "User")));

        yield return A(
            "{__typename: User, id: \"42\"}",
            CreateRepresentation(("__typename", "User"), ("id", "42")));

        yield return A(
            "{__typename: User, iid: 42}",
            CreateRepresentation(("__typename", "User"), ("iid", 42)));

        yield return A(
            "{__typename: User, id: \"42\", iid: 42}",
            CreateRepresentation(("__typename", "User"), ("id", "42"), ("iid", 42)));

        yield return A(
            "{__typename: User, id: \"42\", nested: {iid: 42}}",
            CreateRepresentation(
                ("__typename", "User"),
                ("id", "42"),
                ("nested",
                    CreateRepresentation(("iid", 42)))));

        yield return A(
            "{__typename: User, id: \"42\", nested: {again: {iid: 42}}}",
            CreateRepresentation(
                ("__typename", "User"),
                ("id", "42"),
                ("nested",
                    CreateRepresentation(("again",
                        CreateRepresentation(("iid", 42)))))));

        object[] A(string representation, IReadOnlyDictionary<string, object?> expected)
        {
            return new object[] { representation, expected };
        }
    }
}