using AutoMapper;
using kidsApp.Application.DTOs.StoryProgress_DTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
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
            return _mapper.Map<IEnumerable<StoryProgressDTO>>(progresses);
        }

        public async Task<StoryProgressDTO?> GetByIdAsync(int id)
        {
            var progress = await _unitOfWork.StoryProgress.GetByIdWithDetailsAsync(id);
            return progress == null ? null : _mapper.Map<StoryProgressDTO>(progress);
        }

        public async Task<IEnumerable<StoryProgressDTO>> GetStoryProgressByIdAsync(int storyId)
        {
            var progresses = await _unitOfWork.StoryProgress.GetAllWithDetailsAsync();

            var filtered = progresses
                .Where(p => p.StoryId == storyId)
                .OrderByDescending(p => p.LastUpdated)
                .ToList();

            return _mapper.Map<IEnumerable<StoryProgressDTO>>(filtered);
        }

        public async Task<IEnumerable<StoryProgressDTO>> GetProgressByChildIdAsync(int childId)
        {
            var progresses = await _unitOfWork.StoryProgress.GetAllWithDetailsAsync();

            var filtered = progresses
                .Where(p => p.ChildId == childId)
                .OrderByDescending(p => p.LastUpdated)
                .ToList();

            return _mapper.Map<IEnumerable<StoryProgressDTO>>(filtered);
        }

        public async Task<StoryProgressDTO> CreateAsync(CreateStoryProgressDTO dto)
        {
            var entity = _mapper.Map<StoryProgress>(dto);

            entity.LastUpdated = DateTime.UtcNow;
            entity.Status = entity.ProgressPercent >= 100 ? "Completed" : "InProgress";
            entity.DateCompleted = entity.ProgressPercent >= 100 ? DateTime.UtcNow : default(DateTime);

            await _unitOfWork.StoryProgress.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.StoryProgress.GetByIdWithDetailsAsync(entity.Id);

            return _mapper.Map<StoryProgressDTO>(created!);
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