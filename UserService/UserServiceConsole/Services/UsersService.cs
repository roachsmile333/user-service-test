using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using UserService.Models.User;
using UserServiceConsole.DbContexts;

namespace UserService.Console.Service
{
    public class UsersService
    {
        private readonly MongoDbContext _dbContext;
        private readonly ILogger _logger;
        public UsersService(ILogger<UsersService> logger, MongoDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Asynchronously create user in database
        /// </summary>
        /// <returns>Task result</returns>
        public async Task CreateUserAsync(string username)
        {
            try
            {
                var user = new UserDto() { CreatedDate = DateTime.Now, UserName = username };
                await _dbContext.userCollection.InsertOneAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateUser operation error occurred by next: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Synchronously create user in database
        /// </summary>
        /// <param name="username"></param>
        public void CreateUser(string username)
        {
            try
            {
                var user = new UserDto() { CreatedDate = DateTime.Now, UserName = username };
                _dbContext.userCollection.InsertOne(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateUser operation error occurred by next: {ex.Message}");
                throw;
            }
        }
    }
}
