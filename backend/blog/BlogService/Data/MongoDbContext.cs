using BlogService.Models;
using MongoDB.Driver;

namespace BlogService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);

            ConfigureIndexes();
        }

        public IMongoCollection<Blog> Blogs => _database.GetCollection<Blog>("Blogs");

        private void ConfigureIndexes()
        {
            var likeIndexModel = new CreateIndexModel<Blog>(
                Builders<Blog>.IndexKeys.Ascending("Likes.Username")
                    .Ascending(b => b.Id),
                new CreateIndexOptions { Unique = true, Sparse = true }
            );

            var authorIndexModel = new CreateIndexModel<Blog>(
                Builders<Blog>.IndexKeys.Ascending(b => b.AuthorUsername)
            );

            Blogs.Indexes.CreateMany(new[] { likeIndexModel, authorIndexModel });
        }
    }
}
