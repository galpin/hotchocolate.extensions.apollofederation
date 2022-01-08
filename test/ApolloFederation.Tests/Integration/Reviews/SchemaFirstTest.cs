using System.Collections.Generic;
using System.Linq;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloFederation.Integration.Reviews;

public class SchemaFirstTest : ReviewsTestBase
{
    protected override IRequestExecutorBuilder CreateRequestExecutorBuilder()
    {
        return new ServiceCollection()
            .AddSingleton<UserRepository>()
            .AddSingleton<ReviewRepository>()
            .AddGraphQL()
            .AddApolloFederation()
            .AddDocumentFromString(@"
                type Review @key(fields: ""id"") {
                    id: ID!
                    body: String
                    author: User @provides(fields: ""username"")
                    product: Product
                }

                type User @extends @key(fields: ""id"") {
                    id: ID! @external
                    username: String @external
                    reviews: [Review]
                }

                type Product @extends @key(fields: ""upc"") {
                    upc: String! @external
                    reviews: [Review]
                }

                type Query
            ")
            .BindRuntimeType<User>()
            .BindRuntimeType<Review>()
            .BindRuntimeType<Product>();
    }

    private sealed record Product(string Upc)
    {
        public IReadOnlyList<Review> GetReviews([Service] ReviewRepository reviews)
        {
            return reviews.GetByProductUpc(Upc);
        }

        public static Product ResolveEntity(IEntityResolverContext ctx)
        {
            return new Product(ctx.Representation.GetValue<string>("upc"));
        }
    }

    private sealed record User(string Id, string? Username = null)
    {
        public IReadOnlyList<Review> GetReviews([Service] ReviewRepository reviews)
        {
            return reviews.GetByAuthorId(Id);
        }

        public static User? ResolveEntity(IEntityResolverContext ctx)
        {
            var users = ctx.Service<UserRepository>();
            return users.FindById(ctx.Representation.GetValue<string>("id"));
        }
    }

    private sealed record Review(string Id, string AuthorId, Product Product, string Body, User Author)
    {
        public static Review? ResolveEntity(IEntityResolverContext ctx)
        {
            var reviews = ctx.Service<ReviewRepository>();
            return reviews.FindById(ctx.Representation.GetValue<string>("id"));
        }
    }

    private sealed class ReviewRepository
    {
        private static readonly Review[] s_reviews =
        {
            new(Id: "1", AuthorId: "1", new Product(Upc: "1"), Body: "Love it!", Author: new User("1")),
            new(Id: "2", AuthorId: "1", new Product(Upc: "2"), Body: "Too expensive.", Author: new User("1")),
            new(Id: "3", AuthorId: "2", new Product(Upc: "3"), Body: "Could be better.", Author: new User("2")),
            new(Id: "4", AuthorId: "2", new Product(Upc: "1"), Body: "Prefer something better.", Author: new User("2"))
        };

        public IReadOnlyList<Review> GetByAuthorId(string? id)
        {
            return s_reviews.Where(x => x.AuthorId == id).ToList();
        }

        public IReadOnlyList<Review> GetByProductUpc(string upc)
        {
            return s_reviews.Where(x => x.Product.Upc == upc).ToList();
        }

        public Review? FindById(string id)
        {
            return s_reviews.SingleOrDefault(x => x.Id == id);
        }
    }

    private sealed class UserRepository
    {
        private static readonly User[] s_users =
        {
            new(Id: "1", Username: "@ada"),
            new(Id: "2", Username: "@complete")
        };

        public User? FindById(string? id)
        {
            return s_users.SingleOrDefault(x => x.Id == id);
        }
    }
}