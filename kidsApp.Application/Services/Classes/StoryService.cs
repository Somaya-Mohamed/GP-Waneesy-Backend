using AutoMapper;
using kidsApp.Application.DTOs.StoryDTOs;
using kidsApp.Application.DTOs.StoryProgress_DTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Classes
{
    public class StoryService : IStoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET all stories
        public async Task<IEnumerable<StoryDTO>> GetAllAsync()
        {
            var stories = await _unitOfWork.Stories.GetAllAsync();
            return _mapper.Map<IEnumerable<StoryDTO>>(stories);
        }

        // GET story by id
        public async Task<StoryDTO> GetByIdAsync(int id)
        {
            var story = await _unitOfWork.Stories.GetByIdAsync(id);
            if (story == null) return null;

            return _mapper.Map<StoryDTO>(story);
        }

        // CREATE story
        public async Task<StoryDTO> CreateAsync(CreateStoryDTO dto)
        {
            // Use AutoMapper (mapping already defined in MappingProfile)
            var story = _mapper.Map<Story>(dto);

            await _unitOfWork.Stories.AddAsync(story);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StoryDTO>(story);
        }

        // UPDATE story
        public async Task<bool> UpdateAsync(int id, UpdateStoryDTO dto)
        {
            var story = await _unitOfWork.Stories.GetByIdAsync(id);
            if (story == null) return false;

            // AutoMapper handles StoryText -> Content and Difficulty -> Category
            _mapper.Map(dto, story);

            _unitOfWork.Stories.Update(story);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // DELETE story
        public async Task<bool> DeleteAsync(int id)
        {
            var story = await _unitOfWork.Stories.GetByIdAsync(id);
            if (story == null) return false;

            _unitOfWork.Stories.Delete(story);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // Get story progress by storyId
        public async Task<IEnumerable<StoryProgressDTO>> GetStoryProgressByIdAsync(int storyId)
        {
            var progresses = await _unitOfWork.StoryProgress.GetAllAsync();
            var filtered = progresses.Where(p => p.StoryId == storyId);

            return _mapper.Map<IEnumerable<StoryProgressDTO>>(filtered);
        }

        // Get stories by category
        public async Task<IEnumerable<StoryDTO>> GetStoriesByCategoryAsync(string category)
        {
            var stories = await _unitOfWork.Stories.GetAllAsync();
            var filtered = stories.Where(s => s.Category.ToLower() == category.ToLower());

            return _mapper.Map<IEnumerable<StoryDTO>>(filtered);
        }
    }
}