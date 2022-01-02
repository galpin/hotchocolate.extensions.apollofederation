using System.Threading.Tasks;

namespace HotChocolate.Extensions.ApolloFederation;

internal static class SchemaExtensions
{
    public static async Task QuerySdlAndMatchSnapshotAsync(this ISchema schema)
    {
        await schema.ExecuteAndMatchSnapshotAsync(@"
        {
            _service {
                sdl
            }
        }
        ");
    }
}