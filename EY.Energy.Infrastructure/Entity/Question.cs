using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.Energy.Entity
{
    public class Question
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Label { get; set; } = string.Empty;  
        public List<SubQuestion>? SubQuestions { get; set; }
        public List<ObjectId>? OptionsId { get; set; }
    }
}
