namespace HotChocolate.Extensions.ApolloSubgraph;

internal sealed class DirectiveTypeInterceptorField
{
    public DirectiveTypeInterceptorField(string name, string? fieldSet = null)
    {
        Name = name;
        FieldSet = fieldSet;
    }

    public string Name { get; }

    public string? FieldSet { get; }
}