using kidsApp.Application.DTOs.AdminDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminUserDTO>> GetAllUsersAsync();
        Task<AdminUserDTO?> GetUserByIdAsync(string id);           
        Task<AdminUserDTO> CreateUserAsync(AdminCreateUserDTO dto);
        Task<bool> UpdateUserAsync(string id, AdminUpdateUserDTO dto);  
        Task<bool> DeleteUserAsync(string id);                     
        Task<IEnumerable<AdminRoleDTO>> GetAllRolesAsync();
        Task<AdminRoleDTO> CreateRoleAsync(AdminCreateRoleDTO dto);
    }
}