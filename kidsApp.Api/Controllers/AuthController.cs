using Microsoft.AspNetCore.Mvc;
using kidsApp.Application.DTOs.AuthDTOs;
using kidsApp.Domain.Contracts;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using kidsApp.Domain.Entities;

namespace kidsApp.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        // ====================== Parent Login ======================
        [HttpPost("parent/login")]
        [AllowAnonymous]
        public async Task<IActionResult> ParentLogin([FromBody] ParentLoginDto dto)
        {
            var parent = (await _unitOfWork.Parents.GetAllAsync())
                .FirstOrDefault(p => p.Email == dto.Email && p.Password == dto.Password);

            if (parent == null)
                return Unauthorized(new { Success = false, Message = "Invalid email or password" });

            var token = GenerateJwtToken(parent.Id.ToString(), "Parent", parent.FullName, parent.Id);

            return Ok(new
            {
                Success = true,
                Message = "Parent login successful",
                Token = token,
                Role = "Parent",
                ParentId = parent.Id,
                FullName = parent.FullName
            });
        }

        // ====================== Child Login (PIN Code) ======================
        [HttpPost("child/login")]
        [AllowAnonymous]
        public async Task<IActionResult> ChildLogin([FromBody] ChildLoginDto dto)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(dto.ChildId);

            if (child == null)
                return Unauthorized(new { Success = false, Message = "Child not found" });

            if (child.PinCode != dto.PinCode)
                return Unauthorized(new { Success = false, Message = "Invalid PIN code" });

            var token = GenerateJwtToken(child.Id.ToString(), "Child", child.Name, child.ParentId);

            return Ok(new
            {
                Success = true,
                Message = "Child login successful",
                Token = token,
                Role = "Child",
                ChildId = child.Id,
                ParentId = child.ParentId,
                FullName = child.Name
            });
        }

        // ====================== Switch Child ======================
        [HttpPost("switch-child")]
        public async Task<IActionResult> SwitchChild([FromBody] ChildLoginDto dto)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(dto.ChildId);

            if (child == null || child.PinCode != dto.PinCode)
                return Unauthorized(new { Success = false, Message = "Invalid child or PIN code" });

            var token = GenerateJwtToken(child.Id.ToString(), "Child", child.Name, child.ParentId);

            return Ok(new
            {
                Success = true,
                Message = "Switched to child successfully",
                Token = token,
                ChildId = child.Id,
                ParentId = child.ParentId,
                FullName = child.Name
            });
        }

        // ====================== Admin Login (ثابت في الكود دلوقتي) ======================
        [HttpPost("admin/login")]
        [AllowAnonymous]
        public IActionResult AdminLogin([FromBody] AdminLoginDto dto)
        {
            if (dto.Email == "admin@waneesy.com" && dto.Password == "waneesy123")
            {
                var token = GenerateJwtToken("0", "Admin", "System Admin", 0);

                return Ok(new
                {
                    Success = true,
                    Message = "Admin login successful",
                    Token = token,
                    Role = "Admin",
                    FullName = "System Admin"
                });
            }

            return Unauthorized(new { Success = false, Message = "Invalid admin credentials" });
        }
        //Get Current User(ME)
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var name = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                Id = id,
                Role = role,
                FullName = name
            });
        }

        //Get My Children(Parent Only)
        [HttpGet("my-children")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetMyChildren()
        {
            var parentId = int.Parse(User.FindFirst("ParentId").Value);

            var children = (await _unitOfWork.Children.GetAllAsync())
                .Where(c => c.ParentId == parentId)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Age
                });

            return Ok(new
            {
                Success = true,
                Data = children
            });
        }
        //Change Password(Parent)
        [HttpPost("change-password")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var parentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var parent = await _unitOfWork.Parents.GetByIdAsync(parentId);

            if (parent == null)
                return NotFound(new { Success = false, Message = "Parent not found" });

            if (parent.Password != dto.OldPassword)
                return BadRequest(new { Success = false, Message = "Old password is incorrect" });

            parent.Password = dto.NewPassword;

            await _unitOfWork.CompleteAsync();

            return Ok(new
            {
                Success = true,
                Message = "Password changed successfully"
            });
        }
        //Delete Account(Parent)
        [HttpDelete("delete-account")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> DeleteAccount()
        {
            var parentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var parent = await _unitOfWork.Parents.GetByIdAsync(parentId);

            if (parent == null)
                return NotFound();

            // ممكن هنا كمان تمسح children لو موجودين
            var children = (await _unitOfWork.Children.GetAllAsync())
                .Where(c => c.ParentId == parentId);

            foreach (var child in children)
            {
                _unitOfWork.Children.Delete(child);
            }

            _unitOfWork.Parents.Delete(parent);

            await _unitOfWork.CompleteAsync();

            return Ok(new
            {
                Success = true,
                Message = "Account deleted successfully"
            });
        }
        private string GenerateJwtToken(string id, string role, string name, int parentId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, name),
                new Claim("ParentId", parentId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "super-secret-key-12345"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}