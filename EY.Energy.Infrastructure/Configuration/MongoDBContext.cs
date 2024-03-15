using EY.Energy.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.Energy.Infrastructure.Configuration
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<Company> Companies => _database.GetCollection<Company>("Companies");
        public IMongoCollection<Invoice> Invoices => _database.GetCollection<Invoice>("Invoices");
        public IMongoCollection<Option> Options => _database.GetCollection<Option>("Options");
        public IMongoCollection<Question> Questions => _database.GetCollection<Question>("Questions");
        public IMongoCollection<Response> Responses => _database.GetCollection<Response>("Responses");
        public IMongoCollection<SubQuestion> SubQuestions => _database.GetCollection<SubQuestion>("SubQuestions");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

       

    }
}
