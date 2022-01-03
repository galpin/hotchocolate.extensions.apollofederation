using System.Threading.Tasks;

namespace HotChocolate.Extensions.ApolloFederation.Resolvers;

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

    protected static async Task QueryProductInterfaceAndMatchSnapshotAsync(ISchema schema)
    {
        await schema.ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [{ __typename: ""IProduct"" }]) {
                ...on IProduct {
                    __typename
                    upc
                }
            }
        }
        ");
    }
}