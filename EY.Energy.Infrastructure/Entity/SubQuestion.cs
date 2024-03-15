using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.Energy.Entity
{
    public class SubQuestion
    {

        [BsonId]
        public ObjectId Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public List<ObjectId>? OptionsId { get; set; }
    }
}
