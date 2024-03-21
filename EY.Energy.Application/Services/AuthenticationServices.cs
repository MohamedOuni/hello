using EY.Energy.Entity;
using EY.Energy.Infrastructure.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.Energy.Application.Services
{
    public class AuthenticationServices
    {
        private readonly IMongoCollection<User> _users;

        public AuthenticationServices(MongoDBContext context)
        {
            _users = context.Users;
        }
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _users.Find(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
            return user;
        }
        public async Task<User> GetUserById(string userId)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(userId, out objectId))
            {
                return null!;
            }
            var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
            var user = await _users.Find(filter).FirstOrDefaultAsync();

            return user;
        }
        public async Task<User> GetUserByUsername(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsersExcludingCurrentUser(string currentUsername)
        {
            return await _users.Find(u => u.Username != currentUsername).ToListAsync();
        }


        public async Task UpdateUser(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
        public async Task CreateUser(User user)
        {
            await _users.InsertOneAsync(user);
        }
    }
}
