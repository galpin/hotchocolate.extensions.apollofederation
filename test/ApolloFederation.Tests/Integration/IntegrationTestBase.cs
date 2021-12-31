using System.Threading.Tasks;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Snapshooter;

namespace HotChocolate.Extensions.ApolloFederation.Integration;

public abstract class IntegrationTestBase
{
    protected abstract IRequestExecutorBuilder CreateRequestExecutorBuilder();

    protected async Task ExecuteAndMatchSnapshotAsync(string query)
    {
        var executor = await CreateRequestExecutorBuilder().BuildRequestExecutorAsync();
        var snapshotNameExtension = SnapshotNameExtension.Create(GetType().Name);
        await executor.ExecuteAndMatchSnapshotAsync(query, snapshotNameExtension);
    }
}