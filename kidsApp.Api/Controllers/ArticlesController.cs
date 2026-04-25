using kidsApp.Application.DTOs.ArticleDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/articles")]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ArticlesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/articles
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _serviceManager.ArticleService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Articles retrieved successfully",
                Data = articles
            });
        }

        // GET: api/v1/articles/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _serviceManager.ArticleService.GetByIdAsync(id);
            if (article == null)
                return NotFound(new { Success = false, Message = "Article not found" });

            return Ok(new
            {
                Success = true,
                Message = "Article retrieved successfully",
                Data = article
            });
        }

        // POST: api/v1/articles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateArticleDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var created = await _serviceManager.ArticleService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                new
                {
                    Success = true,
                    Message = "Article created successfully",
                    Data = created
                });
        }

        // PUT: api/v1/articles/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateArticleDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.ArticleService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Article not found" });

            return Ok(new { Success = true, Message = "Article updated successfully" });
        }

        // DELETE: api/v1/articles/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.ArticleService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Article not found" });

            return Ok(new { Success = true, Message = "Article deleted successfully" });
        }

        // GET: api/v1/articles/search/{name}
        [HttpGet("search/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { Success = false, Message = "Search term is required" });

            var result = await _serviceManager.ArticleService.SearchByNameAsync(name);
            return Ok(new
            {
                Success = true,
                Message = "Search completed successfully",
                Data = result
            });
        }
    }
}