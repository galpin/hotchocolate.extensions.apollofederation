using HotChocolate.Language;
using HotChocolate.Types;
using static HotChocolate.Extensions.ApolloSubgraph.ThrowHelper;

namespace HotChocolate.Extensions.ApolloSubgraph;

/// <summary>
/// The <c>_FieldSet</c> type is used to represent a set of fields. A field set is a selection set minus the braces.
/// </summary>
public sealed class FieldSetType : ScalarType<SelectionSetNode, StringValueNode>
{
    /// <summary>
    /// Initializes a new instance of <see cref="FieldSetType"/>.
    /// </summary>
    public FieldSetType()
        : base(Names.FieldSet)
    {
    }

    /// <inheritdoc />
    protected override SelectionSetNode ParseLiteral(StringValueNode valueSyntax)
    {
        try
        {
            return ParseSelectionSet(valueSyntax.Value);
        }
        catch (SyntaxException)
        {
            throw FieldSet_InvalidFormat(this);
        }
    }

    /// <inheritdoc />
    protected override StringValueNode ParseValue(SelectionSetNode runtimeValue)
    {
        return new StringValueNode(SerializeSelectionSet(runtimeValue));
    }

    /// <inheritdoc />
    public override IValueNode ParseResult(object? resultValue)
    {
        return resultValue switch
        {
            null => NullValueNode.Default,
            string s => new StringValueNode(s),
            SelectionSetNode selectionSet => new StringValueNode(SerializeSelectionSet(selectionSet)),
            _ => throw FieldSet_CannotParseValue(this, resultValue.GetType())
        };
    }

    /// <inheritdoc />
    public override bool TrySerialize(object? runtimeValue, out object? resultValue)
    {
        switch (runtimeValue)
        {
            case null:
                resultValue = null;
                return true;
            case SelectionSetNode selectionSet:
                resultValue = SerializeSelectionSet(selectionSet);
                return true;
            default:
                resultValue = null;
                return false;
        }
    }

    /// <inheritdoc />
    public override bool TryDeserialize(object? resultValue, out object? runtimeValue)
    {
        switch (resultValue)
        {
            case null:
                runtimeValue = null;
                return true;
            case SelectionSetNode selectionSet:
                runtimeValue = selectionSet;
                return true;
            case string serializedSelectionSet:
                try
                {
                    runtimeValue = ParseSelectionSet(serializedSelectionSet);
                    return true;
                }
                catch (SyntaxException)
                {
                    runtimeValue = null;
                    return false;
                }
            default:
                runtimeValue = null;
                return false;
        }
    }

    private static SelectionSetNode ParseSelectionSet(string s)
    {
        return Utf8GraphQLParser.Syntax.ParseSelectionSet($"{{{s}}}");
    }

    private static string SerializeSelectionSet(SelectionSetNode selectionSet)
    {
        var s = selectionSet.ToString(indented: false);
        return s.Substring(1, s.Length - 2).Trim();
    }

    internal static class Names
    {
        public const string FieldSet = "_FieldSet";
    }
}