using System.Threading.Tasks;
using Xunit;

namespace HotChocolate.Extensions.ApolloFederation.Integration.Reviews;

/// <summary>
/// https://github.com/apollographql/federation-demo/blob/master/services/reviews/index.js
/// </summary>
public abstract class ReviewsTestBase : IntegrationTestBase
{
    [Fact]
    public async Task Service_sdl()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _service {
                sdl
            }
        }
        ");
    }

    [Fact]
    public async Task Entities_when_one()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { __typename: ""User"", id: ""1"" }
            ]) {
                ...on User {
                    __typename
                    id
                    username
                }
            }
        }
        ");
    }

    [Fact]
    public async Task Entities_when_many()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { __typename: ""User"", id: ""1"" },
                { __typename: ""User"", id: ""2"" }
            ]) {
                ...on User {
                    __typename
                    id
                    username
                }
            }
        }
        ");
    }

    [Fact]
    public async Task Entities_when_mixed()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { __typename: ""User"", id: ""1"" },
                { __typename: ""Product"", upc: ""1"" },
                { __typename: ""Review"", id: ""3"" }
            ]) {
                ...on User {
                    __typename
                    id
                    username
                }
                ...on Product {
                    __typename
                    upc
                }
                ...on Review {
                    __typename
                    id
                }
            }
        }
        ");
    }

    [Fact]
    public async Task Entities_when_not_found()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { __typename: ""NotUser"" }
             ]) {
                ...on User {
                    __typename
                }
            }
        }
        ");
    }

    [Fact]
    public async Task Entities_when_typename_missing()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { id: ""1"" }
             ]) {
                ...on User {
                    __typename
                }
            }
        }
        ");
    }

    [Fact]
    public async Task Entities_when_typename_not_string()
    {
        await ExecuteAndMatchSnapshotAsync(@"
        {
            _entities(representations: [
                { __typename: 42 }
             ]) {
                ...on User {
                    __typename
                }
            }
        }
        ");
    }
}