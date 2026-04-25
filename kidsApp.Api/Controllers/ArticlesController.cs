using kidsApp.Application.DTOs.ArticleDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/articles")]
    [AllowAnonymous]
    public class ArticlesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ArticlesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // ================= GET ALL =================
        [HttpGet]
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

        // ================= GET BY ID =================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _serviceManager.ArticleService.GetByIdAsync(id);

            if (article == null)
                return NotFound(new
                {
                    Success = false,
                    Message = $"Article with Id {id} not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Article retrieved successfully",
                Data = article
            });
        }

        // ================= CREATE =================
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateArticleDTO dto)
        {
            var created = await _serviceManager.ArticleService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new
                {
                    Success = true,
                    Message = "Article created successfully",
                    Data = created
                });
        }

        // ================= UPDATE =================
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateArticleDTO dto)
        {
            var updated = await _serviceManager.ArticleService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new
                {
                    Success = false,
                    Message = $"Article with Id {id} not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Article updated successfully"
            });
        }

        // ================= DELETE =================
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.ArticleService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new
                {
                    Success = false,
                    Message = $"Article with Id {id} not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Article deleted successfully"
            });
        }

        // ================= SEARCH =================
        [HttpGet("search/{name}")]
        public async Task<IActionResult> Search(string name)
        {
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