using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.Energy.Entity
{
    public class Category
    {
        [BsonId]
        public ObjectId Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public List<ObjectId>? QuestionIds { get; set; }
    }
}
