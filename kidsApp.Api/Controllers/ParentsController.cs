using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/parents")]
    [Authorize]
    public class ParentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ParentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/parents
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        // Admin يشوف أي parent — Parent يشوف نفسه بس
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Parent")]
        public async Task<IActionResult> GetById(int id)
        {
            if (User.IsInRole("Parent"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (userId != id)
                    return Forbid();
            }

            var parent = await _serviceManager.ParentService.GetByIdAsync(id);
            if (parent == null)
                return NotFound(new { Success = false, Message = "Parent not found" });

            return Ok(new
            {
                Success = true,
                Message = "Parent retrieved successfully",
                Data = parent
            });
        }

        // POST: api/v1/parents  (Registration — anonymous)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] ParentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var created = await _serviceManager.ParentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ParentId },
                new { Success = true, Message = "Parent created successfully", Data = created });
        }

        // PUT: api/v1/parents/5
        // Parent يعدل بيانات نفسه بس
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateParentDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (userId != id)
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.ParentService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Parent not found" });

            return Ok(new
            {
                Success = true,
                Message = "Parent updated successfully"
            });
        }

        // DELETE: api/v1/parents/5
        // Admin يحذف أي parent — Parent يحذف حسابه بس
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Parent")]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.IsInRole("Parent"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (userId != id)
                    return Forbid();
            }

            var deleted = await _serviceManager.ParentService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Parent not found" });

            return Ok(new
            {
                Success = true,
                Message = "Parent deleted successfully"
            });
        }

        // ================= Advanced =================

        // GET: api/v1/parents/5/children
        // Parent يشوف أولاده بس
        [HttpGet("{id:int}/children")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetChildren(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (userId != id)
                return Forbid();

            var children = await _serviceManager.ParentService.GetChildrenSummaryAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Children retrieved successfully",
                Data = children
            });
        }
    }
}