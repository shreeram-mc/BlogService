using System;
using System.Threading.Tasks;
using BlogService.Models;
using BlogService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesInfoRepository _repo;

        public ArticlesController(IArticlesInfoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]   
        [Route("{name}")]
        public async Task<ActionResult<ArticlesInfo>> Get(string name)
        {
            return Ok(await _repo.Get(name));
        }

        
        [HttpGet]        
        public async Task<ActionResult<ArticlesInfo>> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        
        [HttpPost]
        public async Task<ActionResult<ArticlesInfo>> Create(ArticlesInfo info)
        {
            try
            {
                await _repo.Create(info);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<ActionResult<bool>> Update(ArticlesInfo info)
        {   
                return Ok(await _repo.Update(info));
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(ArticlesInfo info)
        {
            return Ok(await _repo.Delete(info.Name));
        }



        [HttpGet("{name}/upvote")]
        public async Task<ActionResult<bool>> UpVote(string name)
        {
            return Ok(await _repo.UpVote(name));
        }
    }
}