using Microsoft.AspNetCore.Mvc;
using kidsApp.Application.DTOs.AuthDTOs;
using kidsApp.Domain.Contracts;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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