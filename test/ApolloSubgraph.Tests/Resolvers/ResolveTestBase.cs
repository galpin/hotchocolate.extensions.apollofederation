using System.Threading.Tasks;

namespace HotChocolate.Extensions.ApolloSubgraph.Resolvers;

public abstract class ResolveTestBase
{
    protected static async Task QueryProductAndMatchSnapshotAsync(ISchema schema)
    {
        await schema.ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [{ __typename: ""Product"" }]) {
                ...on Product {
                    __typename
                    upc
                }
            }
        }
        ");
    }
}