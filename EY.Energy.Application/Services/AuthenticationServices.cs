using EY.Energy.Entity;
using EY.Energy.Infrastructure.Configuration;
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

        public async Task<User> GetUserByUsername(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
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
