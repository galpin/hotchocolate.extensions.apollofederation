using System.Threading.Tasks;
using HotChocolate.Execution;
using Snapshooter;
using Snapshooter.Xunit;

namespace HotChocolate.Extensions.ApolloFederation;

internal static class SnapshotExtensions
{
    public static async Task QuerySdlAndMatchSnapshotAsync(this ISchema schema)
    {
        await schema.ExecuteAndMatchSnapshotAsync("{ _service { sdl } }");
    }

    public static async Task ExecuteAndMatchSnapshotAsync(
        this ISchema schema,
        string query,
        SnapshotNameExtension? snapshotNameExtension = null)
    {
        var executor = schema.MakeExecutable();
        await executor.ExecuteAndMatchSnapshotAsync(query, snapshotNameExtension);
    }

    public static async Task ExecuteAndMatchSnapshotAsync(
        this IRequestExecutor executor,
        string query,
        SnapshotNameExtension? snapshotNameExtension = null)
    {
        var result = await executor.ExecuteAsync(query);
        result.MatchSnapshot(snapshotNameExtension);
    }

    private static void MatchSnapshot(
        this IExecutionResult result,
        SnapshotNameExtension? snapshotNameExtension = null)
    {
        var json = result.ToJson().NormalizeEscapedLineEndings();
        if (snapshotNameExtension != null)
        {
            json.MatchSnapshot(snapshotNameExtension);
        }
        else
        {
            json.MatchSnapshot();
        }
    }

    private static string NormalizeEscapedLineEndings(this string text)
    {
        // sdl contains escaped line endings which are not normalized by Snapshooter.
        return text
            .Replace("\\r\\n", "\\n")
            .Replace("\\n\\r", "\\n")
            .Replace("\\r", "\\n");
    }
}