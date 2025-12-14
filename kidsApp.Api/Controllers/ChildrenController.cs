using kidsApp.Application.DTOs.ChildDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.ServiceManager;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{


    [Route("api/v1/children")]
    [ApiController]
    //[Authorize] //  Auth
    public class ChildrenController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        //private readonly IChildService _childService;

        public ChildrenController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            // _childService = childService;
        }

        // GET: api/v1/children
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var children = await _serviceManager.ChildService.GetAllAsync();
            return Ok(new { Success = true, Message = "Children retrieved successfully", Data = children });
        }

        // GET: api/v1/children/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var child = await _serviceManager.ChildService.GetByIdAsync(id);
            if (child == null)
                return NotFound(new { Success = false, Message = "Child not found" });

            return Ok(new { Success = true, Message = "Child retrieved successfully", Data = child });
        }

        // POST: api/v1/children
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChildCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var createdChild = await _serviceManager.ChildService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdChild.Id }, new { Success = true, Message = "Child created successfully", Data = createdChild });
        }

        // PUT: api/v1/children/5
        [HttpPut("{id:int}")]
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
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.ChildService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Child not found" });

            return Ok(new { Success = true, Message = "Child deleted successfully" });
        }

        // GET: api/v1/children/5/weekly-report
        [HttpGet("{id:int}/weekly-report")]
        public async Task<IActionResult> GetWeeklyReport(int id)
        {
            var report = await _serviceManager.ChildService.GetWeeklyReportAsync(id);
            if (report == null)
                return NotFound(new { Success = false, Message = "Child not found or no report available" });

            return Ok(new { Success = true, Message = "Weekly report retrieved successfully", Data = report });
        }

        // GET: api/v1/children/5/activities-summary
        [HttpGet("{id:int}/activities-summary")]
        public async Task<IActionResult> GetActivitiesSummary(int id)
        {
            var summary = await _serviceManager.ChildService.GetChildActivitiesSummaryAsync(id);
            return Ok(new { Success = true, Message = "Activities summary retrieved successfully", Data = summary });
        }

        // GET: api/v1/children/5/top-scores
        [HttpGet("{id:int}/top-scores")]
        public async Task<IActionResult> GetTopScores(int id, [FromQuery] int topCount = 5)
        {
            if (topCount <= 0) topCount = 5;
            if (topCount > 50) topCount = 50; // limit 

            var topScores = await _serviceManager.ChildService.GetTopScoresAsync(id, topCount);
            return Ok(new { Success = true, Message = "Top scores retrieved successfully", Data = topScores });
        }
    }


}

