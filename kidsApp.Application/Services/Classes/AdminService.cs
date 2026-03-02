using AutoMapper;
using kidsApp.Application.DTOs.AdminDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

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
            return _mapper.Map<IEnumerable<AdminUserDTO>>(users);
        }

        public async Task<AdminUserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user == null ? null : _mapper.Map<AdminUserDTO>(user);
        }

        public async Task<AdminUserDTO> CreateUserAsync(AdminCreateUserDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));

            if (!await _roleManager.RoleExistsAsync(dto.Role))
                await _roleManager.CreateAsync(new IdentityRole(dto.Role));

            await _userManager.AddToRoleAsync(user, dto.Role);

            return new AdminUserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = dto.Role
            };
        }

        public async Task<bool> UpdateUserAsync(int id, AdminUpdateUserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            _mapper.Map(dto, user);
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            await _userManager.DeleteAsync(user);
            return true;
        }

        // ===== Roles (AutoMapper هنا) =====

        public async Task<IEnumerable<AdminRoleDTO>> GetAllRolesAsync()
        {
            var roles = _roleManager.Roles.ToList();
            return _mapper.Map<IEnumerable<AdminRoleDTO>>(roles);
        }

        public async Task<AdminRoleDTO> CreateRoleAsync(AdminCreateRoleDTO dto)
        {
            var role = new IdentityRole(dto.Name);

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            return _mapper.Map<AdminRoleDTO>(role);
        }
    }
}