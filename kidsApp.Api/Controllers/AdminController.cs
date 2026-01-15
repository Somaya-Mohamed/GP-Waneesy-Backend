using kidsApp.Application.DTOs.AdminDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    [Authorize(Roles = "Admin")] // Only admins can access
    public class AdminController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AdminController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _serviceManager.AdminService.GetAllUsersAsync();
            return Ok(new
            {
                Success = true,
                Message = "Users retrieved successfully",
                Data = users
            });
        }

        // GET: api/v1/admin/users/5
        [HttpGet("users/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _serviceManager.AdminService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { Success = false, Message = "User not found" });

            return Ok(new
            {
                Success = true,
                Message = "User retrieved successfully",
                Data = user
            });
        }

        // POST: api/v1/admin/users
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] AdminCreateUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var createdUser = await _serviceManager.AdminService.CreateUserAsync(dto);

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, new
            {
                Success = true,
                Message = "User created successfully",
                Data = createdUser
            });
        }

        // PUT: api/v1/admin/users/5
        [HttpPut("users/{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] AdminUpdateUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.AdminService.UpdateUserAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "User not found" });

            return Ok(new { Success = true, Message = "User updated successfully" });
        }

        // DELETE: api/v1/admin/users/5
        [HttpDelete("users/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _serviceManager.AdminService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "User not found" });

            return Ok(new { Success = true, Message = "User deleted successfully" });
        }

        // GET: api/v1/admin/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _serviceManager.AdminService.GetAllRolesAsync();
            return Ok(new { Success = true, Message = "Roles retrieved successfully", Data = roles });
        }

        // POST: api/v1/admin/roles
        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] AdminCreateRoleDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var role = await _serviceManager.AdminService.CreateRoleAsync(dto);
            return Ok(new { Success = true, Message = "Role created successfully", Data = role });
        }
    }
}
