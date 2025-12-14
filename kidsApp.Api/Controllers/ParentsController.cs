using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/parents")]
    //[Authorize]
    public class ParentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ParentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/parents
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parents = await _serviceManager.ParentService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Parents retrieved successfully",
                Data = parents
            });
        }

        // GET: api/v1/parents/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var parent = await _serviceManager.ParentService.GetByIdAsync(id);
            if (parent == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Parent not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Parent retrieved successfully",
                Data = parent
            });
        }

        // POST: api/v1/parents
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState
                });

            var created = await _serviceManager.ParentService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.ParentId },
                new
                {
                    Success = true,
                    Message = "Parent created successfully",
                    Data = created
                });
        }

        // PUT: api/v1/parents/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateParentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState
                });

            var updated = await _serviceManager.ParentService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new
                {
                    Success = false,
                    Message = "Parent not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Parent updated successfully"
            });
        }

        // DELETE: api/v1/parents/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.ParentService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new
                {
                    Success = false,
                    Message = "Parent not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Parent deleted successfully"
            });
        }

        // ================= Advanced Endpoints =================

        // GET: api/v1/parents/5/weekly-report
        [HttpGet("{id:int}/weekly-report")]
        public async Task<IActionResult> GetWeeklyReport(int id)
        {
            var report = await _serviceManager.ParentService.GetWeeklyChildReportsAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Weekly report retrieved successfully",
                Data = report
            });
        }

        // GET: api/v1/parents/5/child-summary
        [HttpGet("{id:int}/child-summary")]
        public async Task<IActionResult> GetChildrenSummary(int id)
        {
            var summary = await _serviceManager.ParentService.GetChildrenSummaryAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Children summary retrieved successfully",
                Data = summary
            });
        }

        // POST: api/v1/parents/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ParentLoginDTO dto)
        {
            var token = await _serviceManager.ParentService.LoginAsync(dto.Email, dto.Password);
            if (token == null)
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid credentials"
                });

            return Ok(new
            {
                Success = true,
                Message = "Login successful",
                Token = token
            });
        }
    }
}
