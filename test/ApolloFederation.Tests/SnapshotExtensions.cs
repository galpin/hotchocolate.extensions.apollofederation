using System.Threading.Tasks;
using HotChocolate.Execution;
using Snapshooter;
using Snapshooter.Xunit;

namespace HotChocolate.Extensions.ApolloFederation;

internal static class SnapshotExtensions
{
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
        var json = result.ToJson();
        if (snapshotNameExtension != null)
        {
            json.MatchSnapshot(snapshotNameExtension);
        }
        else
        {
            json.MatchSnapshot();
        }
    }
}