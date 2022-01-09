using System.Collections.Generic;
using System.Linq;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Extensions.ApolloSubgraph.Integration.Reviews;

public class CodeFirstTest : ReviewsTestBase
{
    protected override IRequestExecutorBuilder CreateRequestExecutorBuilder()
    {
        return new ServiceCollection()
            .AddSingleton<UserRepository>()
            .AddSingleton<ReviewRepository>()
            .AddGraphQL()
            .AddApolloSubgraph()
            .AddType<ReviewType>()
            .AddType<UserType>()
            .AddType<ProductType>()
            .AddQueryType();
    }

    private sealed class ReviewType : ObjectType<Review>
    {
        protected override void Configure(IObjectTypeDescriptor<Review> descriptor)
        {
            descriptor.Key(x => x.Id);
            descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
            descriptor.Field(x => x.Body).Type<StringType>();
            descriptor.Field(x => x.AuthorId).Type<UserType>().Provides("username");
            descriptor.Field(x => x.Product).Type<ProductType>();
            descriptor.ResolveEntity(ctx =>
            {
                var reviews = ctx.Service<ReviewRepository>();
                return reviews.FindById(ctx.Representation.GetValue<string>("id"));
            });
        }
    }

    private sealed class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Extends();
            descriptor.Key(x => x.Id);
            descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
            descriptor.Field(x => x.Username).Type<StringType>().External();
            descriptor.Field(x => x.GetReviews(default!)).Type<ListType<ReviewType>>();
            descriptor.ResolveEntity(ctx =>
            {
                var users = ctx.Service<UserRepository>();
                return users.FindById(ctx.Representation.GetValue<string>("id"));
            });
        }
    }

    private sealed class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Extends();
            descriptor.Key(x => x.Upc);
            descriptor.Field(x => x.Upc).Type<NonNullType<StringType>>().External();
            descriptor.Field(x => x.GetReviews(default!)).Type<ListType<ReviewType>>();
            descriptor.ResolveEntity(ctx => new Product(ctx.Representation.GetValue<string>("upc")));
        }
    }

    public sealed record Product(string Upc)
    {
        public IReadOnlyList<Review> GetReviews([Service] ReviewRepository reviews)
        {
            return reviews.GetByProductUpc(Upc);
        }
    }

    public sealed record User(string Id, string Username)
    {
        public IReadOnlyList<Review> GetReviews([Service] ReviewRepository reviews)
        {
            return reviews.GetByAuthorId(Id);
        }
    }

    public sealed record Review(string Id, string AuthorId, Product Product, string Body);

    public sealed class ReviewRepository
    {
        private static readonly Review[] s_reviews =
        {
            new(Id: "1", AuthorId: "1", new Product(Upc: "1"), Body: "Love it!"),
            new(Id: "2", AuthorId: "1", new Product(Upc: "2"), Body: "Too expensive."),
            new(Id: "3", AuthorId: "2", new Product(Upc: "3"), Body: "Could be better."),
            new(Id: "4", AuthorId: "2", new Product(Upc: "1"), Body: "Prefer something better.")
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
            new(Id: "1", Username: "@ada"),
            new(Id: "2", Username: "@complete")
        };

        public User? FindById(string? id)
        {
            return s_users.SingleOrDefault(x => x.Id == id);
        }
    }
}