using HCAnyType = HotChocolate.Types.AnyType;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// The <c>_Any</c> scalar type is used to pass representations of entities from external services.
/// </summary>
public sealed class AnyType : HCAnyType
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnyType"/> class.
    /// </summary>
    public AnyType()
        : base(Names.Any)
    {
    }

    internal static class Names
    {
        public const string Any = "_Any";
    }
}