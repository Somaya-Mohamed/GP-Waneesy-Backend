using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ParentsController : ControllerBase
    {
        private readonly IParentService _parentService;

        public ParentsController(IParentService parentService)
        {
            _parentService = parentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parents = await _parentService.GetAllAsync();
            return Ok(new { Success = true, Data = parents });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var parent = await _parentService.GetByIdAsync(id);
            if (parent == null) return NotFound(new { Success = false, Message = "Parent not found" });
            return Ok(new { Success = true, Data = parent });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParentCreateDto dto)
        {
            var created = await _parentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ParentId }, new { Success = true, Data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateParentDTO dto)
        {
            var updated = await _parentService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { Success = false, Message = "Parent not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _parentService.DeleteAsync(id);
            if (!deleted) return NotFound(new { Success = false, Message = "Parent not found" });
            return NoContent();
        }

        // Advanced Endpoints

        // GET: api/parent/5/weekly-report
        [HttpGet("{id}/weekly-report")]
        public async Task<IActionResult> GetWeeklyReport(int id)
        {
            var report = await _parentService.GetWeeklyChildReportsAsync(id);
            return Ok(new { Success = true, Data = report });
        }

        // GET: api/parent/5/child-summary
        [HttpGet("{id}/child-summary")]
        public async Task<IActionResult> GetChildrenSummary(int id)
        {
            var summary = await _parentService.GetChildrenSummaryAsync(id);
            return Ok(new { Success = true, Data = summary });
        }

        // POST: api/parent/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ParentLoginDTO dto)
        {
            var token = await _parentService.LoginAsync(dto.Email, dto.Password);
            if (token == null) return Unauthorized(new { Success = false, Message = "Invalid credentials" });
            return Ok(new { Success = true, Token = token });
        }
    }
}
