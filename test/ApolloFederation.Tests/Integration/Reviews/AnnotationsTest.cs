using System.Collections.Generic;
using System.Linq;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloFederation.Integration.Reviews;

public class AnnotationsTest : ReviewsTestBase
{
    protected override IRequestExecutorBuilder CreateRequestExecutorBuilder()
    {
        return new ServiceCollection()
            .AddSingleton<UserRepository>()
            .AddSingleton<ReviewRepository>()
            .AddGraphQL()
            .AddApolloFederation()
            .AddQueryType()
            .AddObjectType<Review>()
            .AddObjectType<User>()
            .AddObjectType<Product>();
    }

    public sealed record Review
    {
        public Review(string id, string authorId, Product product, string body)
        {
            Id = id;
            AuthorId = authorId;
            Product = product;
            Body = body;
        }

        [GraphQLKey]
        public string Id { get; }

        public string AuthorId { get; }

        [GraphQLProvides("username")]
        public Product Product { get;}

        public string Body { get; }

        public static Review? ResolveEntity(IEntityResolverContext context)
        {
            var reviews = context.Service<ReviewRepository>();
            return reviews.FindById(context.Representation.GetValue<string>("id"));
        }
    }

    [GraphQLExtends]
    public sealed record User
    {
        public User(string id, string username)
        {
            Id = id;
            Username = username;
        }

        [GraphQLKey]
        [GraphQLExternal]
        public string Id { get; }

        [GraphQLExternal]
        public string Username { get; }

        public IReadOnlyList<Review> GetReviews([Service] ReviewRepository reviews)
        {
            return reviews.GetByAuthorId(Id);
        }

        public static User? ResolveEntity(IEntityResolverContext context)
        {
            var users = context.Service<UserRepository>();
            return users.FindById(context.Representation.GetValue<string>("id"));
        }
    }

    [GraphQLExtends]
    public sealed record Product
    {
        public Product(string upc)
        {
            Upc = upc;
        }

        [GraphQLKey]
        [GraphQLExternal]
        public string Upc { get; }

        public IReadOnlyList<Review> GetReviews([Service] ReviewRepository reviews)
        {
            return reviews.GetByProductUpc(Upc);
        }

        public static Product ResolveEntity(IEntityResolverContext context)
        {
            return new Product(context.Representation.GetValue<string>("upc"));
        }
    }

    public sealed class ReviewRepository
    {
        private static readonly Review[] s_reviews =
        {
            new(id: "1", authorId: "1", new Product(upc: "1"), body: "Love it!"),
            new(id: "2", authorId: "1", new Product(upc: "2"), body: "Too expensive."),
            new(id: "3", authorId: "2", new Product(upc: "3"), body: "Could be better."),
            new(id: "4", authorId: "2", new Product(upc: "1"), body: "Prefer something better.")
        };

        public IReadOnlyList<Review> GetByAuthorId(string id)
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

    public sealed class UserRepository
    {
        private static readonly User[] s_users =
        {
            new(id: "1", username: "@ada"),
            new(id: "2", username: "@complete")
        };

        public User? FindById(string? id)
        {
            return s_users.SingleOrDefault(x => x.Id == id);
        }
    }
}
