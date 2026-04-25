using kidsApp.Application.DTOs.ChildDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.API.Controllers
{
    [Route("api/v1/children")]
    [ApiController]
    [Authorize]
    public class ChildrenController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ChildrenController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/children
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAll()
        {
            var children = await _serviceManager.ChildService.GetAllAsync();
            return Ok(new { Success = true, Message = "Children retrieved successfully", Data = children });
        }

        // GET: api/v1/children/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Parent")]
        public async Task<IActionResult> GetById(int id)
        {
            var child = await _serviceManager.ChildService.GetByIdAsync(id);
            if (child == null)
                return NotFound(new { Success = false, Message = "Child not found" });

            return Ok(new { Success = true, Message = "Child retrieved successfully", Data = child });
        }

        // POST: api/v1/children
        [HttpPost]
        [Authorize(Roles = "Parent")]

        public async Task<IActionResult> Create([FromBody] ChildCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            if (dto.ParentId <= 0)
                return BadRequest(new { Success = false, Message = "ParentId is required and must be valid" });

            var createdChild = await _serviceManager.ChildService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdChild.Id },
                new { Success = true, Message = "Child created successfully", Data = createdChild });
        }

        // PUT: api/v1/children/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Parent")]

        public async Task<IActionResult> Update(int id, [FromBody] ChildUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.ChildService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Child not found" });

            return Ok(new { Success = true, Message = "Child updated successfully" });
        }

        // DELETE: api/v1/children/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Parent")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.ChildService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Child not found" });

            return Ok(new { Success = true, Message = "Child deleted successfully" });
        }

        // ====================== Advanced Endpoints ======================

        // GET: api/v1/children/5/weekly-report
        [HttpGet("{id:int}/weekly-report")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetWeeklyReport(int id)
        {
            var report = await _serviceManager.ChildService.GetWeeklyReportAsync(id);

            if (report == null)
                return NotFound(new { Success = false, Message = "Child not found or no report available" });

            return Ok(new
            {
                Success = true,
                Message = "Weekly report retrieved successfully",
                Data = report
            });
        }

        // GET: api/v1/children/5/activities-summary   
        [HttpGet("{id:int}/activities-summary")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetActivitiesSummary(int id)
        {
            var summary = await _serviceManager.ChildService.GetChildActivitiesSummaryAsync(id);

            if (summary == null)
                return NotFound(new { Success = false, Message = "Child not found" });

            return Ok(new
            {
                Success = true,
                Message = "Activities summary retrieved successfully",
                Data = summary
            });
        }

        // GET: api/v1/children/5/top-scores
        [HttpGet("{id:int}/top-scores")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetTopScores(int id, [FromQuery] int topCount = 5)
        {
            if (topCount <= 0) topCount = 5;
            if (topCount > 50) topCount = 50;

            var topScores = await _serviceManager.ChildService.GetTopScoresAsync(id, topCount);
            return Ok(new { Success = true, Message = "Top scores retrieved successfully", Data = topScores });
        }


        // POST: api/v1/children/5/avatar   
        [HttpPost("{id:int}/avatar")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> UploadAvatar(int id, [FromForm] ChildAvatarUploadDto dto)
        {
            if (dto.AvatarImage == null || dto.AvatarImage.Length == 0)
                return BadRequest(new { Success = false, Message = "No avatar image uploaded" });

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(dto.AvatarImage.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { Success = false, Message = "Only jpg, jpeg, png, webp allowed" });

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"child_{id}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.AvatarImage.CopyToAsync(stream);
            }

            var avatarUrl = $"/avatars/{uniqueFileName}";   

            var updateDto = new ChildUpdateDto { AvatarUrl = avatarUrl };
            var updated = await _serviceManager.ChildService.UpdateAsync(id, updateDto);

            if (!updated)
                return NotFound(new { Success = false, Message = "Child not found" });

            return Ok(new
            {
                Success = true,
                Message = "Avatar uploaded and updated successfully",
                Data = new { AvatarUrl = avatarUrl }
            });
        }

    }
}