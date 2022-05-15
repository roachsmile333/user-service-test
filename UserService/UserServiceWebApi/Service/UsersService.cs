using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models.User;
using UserServiceWebApi.DbContexts;

namespace UserServiceWebApi.Service
{
    public class UsersService
    {
        private readonly ILogger<UsersService> _logger;
        private readonly QueryPublisherService _publisherService;
        private readonly MongoDbContext _dbContext;
        public UsersService(ILogger<UsersService> logger, QueryPublisherService publisherService, MongoDbContext dbContext)
        {
            _logger = logger;
            _publisherService = publisherService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Asynchronously create user in database
        /// </summary>
        /// <returns>Creation result as boolean flag</returns>
        public async Task<bool> CreateUserAsync(string username)
        {
            try
            {
                //we can use UpdateOne with upsert: true
                //but we don't need to update current user if it exists
                var currentUser = await _dbContext.userCollection.FindAsync(x => x.UserName == username);
                if (currentUser.Any())
                    return false;
                _publisherService.SendCommand(username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateUser operation error occurred by next: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Asynchronously returns users from database
        /// </summary>
        /// <returns>Get result as list of users</returns>
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var result = await _dbContext.userCollection.Find(new BsonDocument()).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllUsers operation error occurred by next: {ex.Message}");
                throw;
            }
        }
    }
}
