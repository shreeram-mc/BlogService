using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace BlogService.Models
{
    public partial class ArticlesInfo
    {        
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public string Text { get; set; }

        [BsonElement]
        public int UpVotes { get; set; }

        [BsonElement]
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }

    public class Comment
    {
        public string UserName { get; set; }
        public string Comments { get; set; }
    }
}
