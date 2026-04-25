using AutoMapper;
using kidsApp.Application.DTOs.AdminDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Classes
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AdminService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // ===== Users =====
        public async Task<IEnumerable<AdminUserDTO>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var result = new List<AdminUserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new AdminUserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? "User"
                });
            }

            return result;
        }

        public async Task<AdminUserDTO?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new AdminUserDTO
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? "User"
            };
        }

        public async Task<AdminUserDTO> CreateUserAsync(AdminCreateUserDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // إنشاء الـ Role لو مش موجود
            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(dto.Role));
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return new AdminUserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = dto.Role
            };
        }

        public async Task<bool> UpdateUserAsync(string id, AdminUpdateUserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            user.UserName = dto.UserName;
            user.Email = dto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return false;

            // تحديث الـ Role لو تم إرسالها
            if (!string.IsNullOrEmpty(dto.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        // ===== Roles =====
        public async Task<IEnumerable<AdminRoleDTO>> GetAllRolesAsync()
        {
            var roles = _roleManager.Roles.ToList();
            return _mapper.Map<IEnumerable<AdminRoleDTO>>(roles);
        }

        public async Task<AdminRoleDTO> CreateRoleAsync(AdminCreateRoleDTO dto)
        {
            if (await _roleManager.RoleExistsAsync(dto.Name))
                throw new Exception("Role already exists");

            var role = new IdentityRole(dto.Name);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            return _mapper.Map<AdminRoleDTO>(role);
        }
    }
}