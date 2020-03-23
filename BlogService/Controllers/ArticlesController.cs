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
                return Ok(GetByNameOrId(null, info.Name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<ActionResult<bool>> Update(ArticlesInfo info)
        {
            try
            {
                if (await _repo.Update(info))
                {
                    return Ok(GetByNameOrId(info.Id));
                }

                return BadRequest("Update Failed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(ArticlesInfo info)
        {
            try
            {
                return Ok(await _repo.Delete(info.Name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{name}/upvote")]
        public async Task<ActionResult<bool>> UpVote(string name)
        {
            try
            {
                if (await _repo.UpVote(name))
                    return Ok(GetByNameOrId(null, name));

                return BadRequest($"Failed to upvote for {name}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("{name}/add-comment")]
        public async Task<ActionResult<bool>> AddComment(string name, Comment comment)
        {
            try
            {
                if (await _repo.UpdateComments(name, comment))
                    return Ok(GetByNameOrId(null, name));

                return BadRequest($"Failed to add comments for {name}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private ArticlesInfo GetByNameOrId(string id = null, string name = null)
        {
            if (id != null)
            {
                return _repo.GetById(id).Result;
            }
            else if (name != null)
                return _repo.Get(name).Result;

            return null;

        }
    }
}