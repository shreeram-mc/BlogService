using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string username = "root";
            string password = "example";
            string mongoDbAuthMechanism = "SCRAM-SHA-1";
            MongoInternalIdentity internalIdentity =
                      new MongoInternalIdentity("admin", username);
            PasswordEvidence passwordEvidence = new PasswordEvidence(password);
            MongoCredential mongoCredential =
                 new MongoCredential(mongoDbAuthMechanism,
                         internalIdentity, passwordEvidence);

            MongoClientSettings settings = new MongoClientSettings
            { 
                Credential = mongoCredential
            };

            var mongoHost = "127.0.0.1";
            MongoServerAddress address = new MongoServerAddress(mongoHost);
            settings.Server = address;

            var client = new MongoClient(settings);

            var db = client.GetDatabase("blogdb");

            var collection = db.GetCollection<ArticlesInfo>("ArticlesInfo");

           var items = collection.Find(_ => true)
                 .ToList();

            items.ForEach(a => Console.WriteLine(a.Comments));

            //Console.WriteLine(collection);

            Console.ReadLine();
        }
    }

    public partial class ArticlesInfo
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        [BsonElement]
        public string Name { get; set; }
        [BsonElement]
        public int UpVotes { get; set; }
        [BsonElement]
        public string Comments { get; set; }

    }
}
