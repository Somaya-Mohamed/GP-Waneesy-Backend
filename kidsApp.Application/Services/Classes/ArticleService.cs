using AutoMapper;
using kidsApp.Application.DTOs.ArticleDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

namespace kidsApp.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDTO>> GetAllAsync()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync();
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }

        public async Task<ArticleDTO?> GetByIdAsync(int id)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            return article == null ? null : _mapper.Map<ArticleDTO>(article);
        }

        public async Task<ArticleDTO> CreateAsync(CreateArticleDTO dto)
        {
            var article = _mapper.Map<Article>(dto);

            await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ArticleDTO>(article);
        }

        public async Task<bool> UpdateAsync(int id, UpdateArticleDTO dto)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            if (article == null) return false;

            _mapper.Map(dto, article);
            _unitOfWork.Articles.Update(article);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            if (article == null) return false;

            _unitOfWork.Articles.Delete(article);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ArticleDTO>> SearchByNameAsync(string name)
        {
            var articles = await _unitOfWork.Articles.GetAllAsync();

            var filtered = articles
                .Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return _mapper.Map<IEnumerable<ArticleDTO>>(filtered);
        }
    }
}