using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.Energy.Entity
{
    public class Response
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Answer { get; set; } = string.Empty;
        public ObjectId QuestionId { get; set; }
        public ObjectId SubQuestionId { get; set; }
        public ObjectId UserId { get; set; }


    }
}
