using AutoMapper;
using kidsApp.Application.DTOs.StoryProgress_DTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

namespace kidsApp.Application.Services.Classes
{
    public class StoryProgressService : IStoryProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StoryProgressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StoryProgressDTO>> GetAllAsync()
        {
            var progresses = await _unitOfWork.StoryProgress.GetAllWithDetailsAsync();

            return progresses.Select(p => new StoryProgressDTO
            {
                ProgressId = p.Id,
                ChildId = p.ChildId,
                ChildName = p.Child.Name,
                StoryId = p.StoryId,
                StoryTitle = p.Story.Title,
                ProgressPercent = p.ProgressPercent,
                LastUpdated = p.LastUpdated
            });
        }

        public async Task<StoryProgressDTO?> GetByIdAsync(int id)
        {
            var progress = await _unitOfWork.StoryProgress.GetByIdWithDetailsAsync(id);
            if (progress == null) return null;

            return new StoryProgressDTO
            {
                ProgressId = progress.Id,
                ChildId = progress.ChildId,
                ChildName = progress.Child.Name,
                StoryId = progress.StoryId,
                StoryTitle = progress.Story.Title,
                ProgressPercent = progress.ProgressPercent,
                LastUpdated = progress.LastUpdated
            };
        }

        public async Task<StoryProgressDTO> CreateAsync(CreateStoryProgressDTO dto)
        {
            var entity = _mapper.Map<StoryProgress>(dto);

            entity.Status = dto.ProgressPercent >= 100 ? "Completed" : "InProgress";
            entity.LastUpdated = DateTime.UtcNow;
            entity.DateCompleted = dto.ProgressPercent >= 100
                ? DateTime.UtcNow
                : default;

            await _unitOfWork.StoryProgress.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var fullEntity = await _unitOfWork.StoryProgress
                .GetByIdWithDetailsAsync(entity.Id);

            return await GetByIdAsync(entity.Id) ?? throw new Exception("Creation failed");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var progress = await _unitOfWork.StoryProgress.GetByIdAsync(id);
            if (progress == null) return false;

            _unitOfWork.StoryProgress.Delete(progress);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}