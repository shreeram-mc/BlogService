using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BlogService.DataContexts
{
    public class ApplicationDbContext<T> : IApplicationDbContext<T>
    {
        private readonly IMongoDatabase _db;
        public ApplicationDbContext(IOptions<Settings> options)
        {
            var client = InitializeDatabase(options);
            _db = client.GetDatabase(options.Value.Database);
        }

        private MongoClient InitializeDatabase(IOptions<Settings> options)
        {
            if (options.Value.IsContained)
            {
                var address = new MongoServerAddress("mongo");
                var mongoDbAuthMechanism = "SCRAM-SHA-1";
                MongoInternalIdentity internalIdentity = new MongoInternalIdentity("admin", options.Value.User);
                PasswordEvidence passwordEvidence = new PasswordEvidence(options.Value.Password);
                MongoCredential mongoCredential = new MongoCredential(mongoDbAuthMechanism, internalIdentity, passwordEvidence);

                MongoClientSettings settings = new MongoClientSettings
                {
                    Credential = mongoCredential,
                    Server = address
                };

                return new MongoClient(settings);
            }

            var mongoClient = new MongoClient(new MongoUrl($@"mongodb://{options.Value.Host}:{options.Value.Port}"));

            return mongoClient;
        }

        public IMongoCollection<T> GetAll
        {
            get
            {
                return _db.GetCollection<T>(typeof(T).Name);
            }
        } 

        public async Task InsertOneAsync(T doc)
        {
            await _db.GetCollection<T>(typeof(T).Name).InsertOneAsync(doc);
        }

        public async Task InsertMany(IEnumerable<T> docs)
        {
            await _db.GetCollection<T>(typeof(T).Name).InsertManyAsync(docs);
        }

        public async Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter)
        {
            return await _db.GetCollection<T>(typeof(T).Name).DeleteOneAsync(filter);
        }

        public async Task<UpdateResult> Update(FilterDefinition<T> filters, UpdateDefinition<T> updates)
        {   
           return await _db.GetCollection<T>(typeof(T).Name).UpdateOneAsync(filters,updates);
        }

 
    }

    public interface IApplicationDbContext<T>
    {
        IMongoCollection<T> GetAll { get; }

        Task InsertOneAsync(T doc);

        Task InsertMany(IEnumerable<T> docs);

        Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter);

        Task<UpdateResult> Update(FilterDefinition<T> filters, UpdateDefinition<T> updates);
    }
}
