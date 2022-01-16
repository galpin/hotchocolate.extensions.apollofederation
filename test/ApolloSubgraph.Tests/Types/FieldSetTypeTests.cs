using System.Threading.Tasks;
using HotChocolate.Language;
using HotChocolate.Types;
using Xunit;
using static HotChocolate.Extensions.ApolloSubgraph.Test;

namespace HotChocolate.Extensions.ApolloSubgraph.Types;

public class FieldSetTypeTests
{
    [Fact]
    public async Task Ctor_correctly_configures_object()
    {
        var schema = await BuildEmptySchemaAsync();

        var sut = schema.GetType<FieldSetType>("_FieldSet");

        Assert.Equal("_FieldSet", sut.Name);
    }

    [Fact]
    public void Deserialize()
    {
        var sut = new FieldSetType();

        var actual = sut.Deserialize("a b c d e(d: $b)");

        Assert.IsType<SelectionSetNode>(actual);
    }

    [Fact]
    public void Deserialize_throws_when_invalid_format()
    {
        var sut = new FieldSetType();

        Assert.Throws<SerializationException>(() => sut.Deserialize("1"));
    }

    [Fact]
    public void TryDeserialize()
    {
        var sut = new FieldSetType();

        var success = sut.TryDeserialize("a b c d e(d: $b)", out var actual);

        Assert.True(success);
        Assert.IsType<SelectionSetNode>(actual);
    }

    [Fact]
    public void TryDeserialize_does_not_throw_when_null()
    {
        var sut = new FieldSetType();

        var success = sut.TryDeserialize(null, out var selectionSet);

        Assert.True(success);
        Assert.Null(selectionSet);
    }

    [Fact]
    public void TryDeserialize_does_not_throw_when_invalid_syntax()
    {
        var sut = new FieldSetType();

        var success = sut.TryDeserialize("1", out var actual);

        Assert.False(success);
        Assert.Null(actual);
    }

    [Fact]
    public void TryDeserialize_does_not_throw_when_invalid_type()
    {
        var sut = new FieldSetType();

        var success = sut.TryDeserialize(1, out var selectionSet);

        Assert.False(success);
        Assert.Null(selectionSet);
    }

    [Fact]
    public void Serialize()
    {
        var sut = new FieldSetType();
        var selectionSet  = Utf8GraphQLParser.Syntax.ParseSelectionSet("{ a b c d e(d: $b) }");

        var actual = sut.Serialize(selectionSet);

        Assert.Equal("a b c d e(d: $b)", actual);
    }

    [Fact]
    public void Serialize_throws_when_invalid_format()
    {
        var sut = new FieldSetType();

        Assert.Throws<SerializationException>(() => sut.Serialize(1));
    }

    [Fact]
    public void TrySerialize()
    {
        var sut = new FieldSetType();
        var selectionSet = Utf8GraphQLParser.Syntax.ParseSelectionSet("{ a b c d e(d: $b) }");

        var success = sut.TrySerialize(selectionSet, out var actual);

        Assert.True(success);
        Assert.Equal("a b c d e(d: $b)", actual);
    }

    [Fact]
    public void TrySerialize_does_not_throw_when_invalid_format()
    {
        var sut = new FieldSetType();

        var success = sut.TrySerialize(1, out var actual);

        Assert.False(success);
        Assert.Null(actual);
    }

    [Fact]
    public void ParseValue()
    {
        var sut = new FieldSetType();
        var selectionSet = Utf8GraphQLParser.Syntax.ParseSelectionSet("{ a b c d e(d: $b) }");

        var actual = sut.ParseValue(selectionSet);

        var node = Assert.IsType<StringValueNode>(actual);
        Assert.Equal("a b c d e(d: $b)", node.Value);
    }

    [Fact]
    public void ParseValue_does_not_throw_when_null()
    {
        var type = new FieldSetType();

        var valueSyntax = type.ParseValue(null);

        Assert.IsType<NullValueNode>(valueSyntax);
    }

    [Fact]
    public void ParseValue_throws_when_invalid_value()
    {
        var type = new FieldSetType();

        Assert.Throws<SerializationException>(() => type.ParseValue(1));
    }
}