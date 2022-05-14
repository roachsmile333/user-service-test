using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserServiceWebApi.Models;

namespace UserServiceWebApi.Service
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly QueryPublisherService _publisherService;
        public UserService(ILogger<UserService> logger, QueryPublisherService publisherService)
        {
            _logger = logger;
            _publisherService = publisherService;
        }

        /// <summary>
        /// Asynchronously create user in database
        /// </summary>
        /// <returns>Creation result as boolean flag</returns>
        public async Task<bool> CreateUserAsync(string username)
        {
            try
            {
                _publisherService.SendMessage($"User {username} was created successful");
                return false;
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
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                _publisherService.SendMessage($"GetAllUsersAsync message");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllUsers operation error occurred by next: {ex.Message}");
                throw;
            }
        }
    }
}
