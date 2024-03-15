using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.Energy.Entity
{
    public class Company
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public List<ObjectId>? AnswerIds { get; set; }

    }
}
