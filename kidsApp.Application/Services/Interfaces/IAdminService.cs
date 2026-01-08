using kidsApp.Application.DTOs.AdminDTOs;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminUserDTO>> GetAllUsersAsync();
        Task<AdminUserDTO> GetUserByIdAsync(int id);
        Task<AdminUserDTO> CreateUserAsync(AdminCreateUserDTO dto);
        Task<bool> UpdateUserAsync(int id, AdminUpdateUserDTO dto);
        Task<bool> DeleteUserAsync(int id);

        Task<IEnumerable<AdminRoleDTO>> GetAllRolesAsync();
        Task<AdminRoleDTO> CreateRoleAsync(AdminCreateRoleDTO dto);
    }
}
