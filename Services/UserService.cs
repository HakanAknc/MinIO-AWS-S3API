﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using S3AdvancedV2.Models;

namespace S3AdvancedV2.Services
{
    public class UserService
    {
        private readonly IMongoCollection<UserModel> _users;

        public UserService(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<UserModel>("Users");
        }

        // This method retrieves a user by their username from the MongoDB collection.
        public async Task<UserModel> GetByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        // This method creates a new user in the MongoDB collection.
        public async Task CreateAsync(UserModel user)
        {
            await _users.InsertOneAsync(user);
        }

        // This method retrieves all users from the MongoDB collection.
        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }
    }
}
