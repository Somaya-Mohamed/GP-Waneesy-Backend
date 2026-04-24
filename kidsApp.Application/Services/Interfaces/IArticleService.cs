using kidsApp.Application.DTOs.ArticleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDTO>> GetAllAsync();
        Task<ArticleDTO?> GetByIdAsync(int id);
        Task<ArticleDTO> CreateAsync(CreateArticleDTO dto);
        Task<bool> UpdateAsync(int id, UpdateArticleDTO dto);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<ArticleDTO>> SearchByNameAsync(string name);
    }
}
