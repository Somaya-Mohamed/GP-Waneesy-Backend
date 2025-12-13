using kidsApp.Application.DTOs.ChildDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildService _childService;

        public ChildrenController(IChildService childService)
        {
            _childService = childService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var children = await _childService.GetAllAsync();
            return Ok(new { Success = true, Data = children });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var child = await _childService.GetByIdAsync(id);
            if (child == null) return NotFound(new { Success = false, Message = "Child not found" });
            return Ok(new { Success = true, Data = child });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChildCreateDTO dto)
        {
            var created = await _childService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.FullName }, new { Success = true, Data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChildUpdateDto dto)
        {
            var updated = await _childService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { Success = false, Message = "Child not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _childService.DeleteAsync(id);
            if (!deleted) return NotFound(new { Success = false, Message = "Child not found" });
            return NoContent();
        }

        // Advanced Endpoints

        // GET: api/child/5/report
        [HttpGet("{id}/report")]
        public async Task<IActionResult> GetWeeklyReport(int id)
        {
            var report = await _childService.GetWeeklyReportAsync(id);
            return Ok(new { Success = true, Data = report });
        }

        // GET: api/child/5/activities-summary
        [HttpGet("{id}/activities-summary")]
        public async Task<IActionResult> GetActivitiesSummary(int id)
        {
            var summary = await _childService.GetChildActivitiesSummaryAsync(id);
            return Ok(new { Success = true, Data = summary });
        }

        // GET: api/child/5/top-scores
        [HttpGet("{id}/top-scores")]
        public async Task<IActionResult> GetTopScores(int id, [FromQuery] int topCount = 5)
        {
            var topScores = await _childService.GetTopScoresAsync(id, topCount);
            return Ok(new { Success = true, Data = topScores });
        }
    }
}
