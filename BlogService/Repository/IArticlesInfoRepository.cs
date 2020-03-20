using BlogService.DataContexts;
using BlogService.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogService.Repository
{
    public interface IArticlesInfoRepository
    {       
        Task<IEnumerable<ArticlesInfo>> GetAll();
         
        Task<ArticlesInfo> Get(string name);
        
        Task Create(ArticlesInfo articlesInfo);
      
        Task<bool> Update(ArticlesInfo articlesInfo);
         
        Task<bool> Delete(string name);

        Task<bool> UpVote(string articleName);
    }

    public class ArticlesInfoRepository : IArticlesInfoRepository
    {
        private readonly IApplicationDbContext<ArticlesInfo> _context;
        public ArticlesInfoRepository(IApplicationDbContext<ArticlesInfo> context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticlesInfo>> GetAll()
        {
            return await _context
                 .GetAll
                 .Find(_ => true)
                 .ToListAsync();
        }

        public async Task<ArticlesInfo> Get(string name)
        {
            FilterDefinition<ArticlesInfo> filter = Builders<ArticlesInfo>.Filter.Eq(m => m.Name, name);
            var result = await _context.GetAll.Find(filter).FirstOrDefaultAsync();

            return result;
        }

        public async Task Create(ArticlesInfo articlesInfo)
        {
            await _context.InsertOneAsync(articlesInfo);
        }

        public async Task<bool> Delete(string name)
        {
            FilterDefinition<ArticlesInfo> filter = Builders<ArticlesInfo>.Filter.Eq(m => m.Name, name);
            DeleteResult deleteResult = await _context.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
        
        public async Task<bool> Update(ArticlesInfo articlesInfo)
        {
            FilterDefinition<ArticlesInfo> filter = Builders<ArticlesInfo>.Filter.Eq(m => m.Id, articlesInfo.Id.ToString());

            var update = Builders<ArticlesInfo>.Update
                        .Set("Name", articlesInfo.Name)
                        .Set("UpVotes", articlesInfo.UpVotes)
                        .Set("Comments", articlesInfo.Comments);

            var result = await _context.Update(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> UpVote(string articleName)
        {
            var article = Get(articleName).Result;

            FilterDefinition<ArticlesInfo> filter = Builders<ArticlesInfo>.Filter.Eq(m => m.Id, article.Id.ToString());

            var count = article.UpVotes + 1;
            var update = Builders<ArticlesInfo>.Update
                        .Set("UpVotes", count);
                        
            var result = await _context.Update(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }


}
