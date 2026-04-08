using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/parents")]
    public class ParentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ParentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parents = await _serviceManager.ParentService.GetAllAsync();
            return Ok(new { Success = true, Message = "Parents retrieved successfully", Data = parents });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var parent = await _serviceManager.ParentService.GetByIdAsync(id);
            if (parent == null)
                return NotFound(new { Success = false, Message = "Parent not found" });

            return Ok(new { Success = true, Message = "Parent retrieved successfully", Data = parent });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var created = await _serviceManager.ParentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ParentId },
                new { Success = true, Message = "Parent created successfully", Data = created });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateParentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.ParentService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Parent not found" });

            return Ok(new { Success = true, Message = "Parent updated successfully" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.ParentService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Parent not found" });

            return Ok(new { Success = true, Message = "Parent deleted successfully" });
        }

        // ================= Advanced Endpoints =================

        [HttpGet("{id:int}/children")]
        public async Task<IActionResult> GetChildren(int id)
        {
            var children = await _serviceManager.ParentService.GetChildrenSummaryAsync(id);
            return Ok(new { Success = true, Message = "Children retrieved successfully", Data = children });
        }

        //[HttpGet("{id:int}/weekly-progress")]
        //public async Task<IActionResult> GetWeeklyProgress(int id)
        //{
        //    var reports = await _serviceManager.ParentService.GetWeeklyChildReportsAsync(id);
        //    return Ok(new { Success = true, Message = "Weekly progress retrieved successfully", Data = reports });
        //}

        //[AllowAnonymous]
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] ParentLoginDTO dto)
        //{
        //    var token = await _serviceManager.ParentService.LoginAsync(dto.Email, dto.Password);
        //    if (token == null)
        //        return Unauthorized(new { Success = false, Message = "Invalid email or password" });

        //    return Ok(new { Success = true, Message = "Login successful", Token = token });
        //}
    }
}