using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UserService.Models.User;

namespace UserServiceWebApi.DbContexts
{
    public class MongoDbContext
    {
        private readonly IConfiguration _configuration;
        public readonly IMongoCollection<UserDto> userCollection;

        public MongoDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient(_configuration.GetSection("MongoDb").GetSection("ConnectionString").Value);
            var database = client.GetDatabase(_configuration.GetSection("MongoDb").GetSection("DatabaseName").Value);

            userCollection = database.GetCollection<UserDto>("Users");
        }
    }
}
