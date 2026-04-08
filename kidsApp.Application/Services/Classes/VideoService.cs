using AutoMapper;
using kidsApp.Application.DTOs.VideoDTOs;
using kidsApp.Application.DTOs.VideoActivityDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
{
    public class VideoService : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VideoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VideoDTO>> GetAllAsync()
        {
            var videos = await _unitOfWork.Videos.GetAllAsync();
            return _mapper.Map<IEnumerable<VideoDTO>>(videos);
        }

        public async Task<VideoDTO?> GetByIdAsync(int id)
        {
            var video = await _unitOfWork.Videos.GetByIdAsync(id);
            return video == null ? null : _mapper.Map<VideoDTO>(video);
        }

        public async Task<VideoDTO> CreateAsync(CreateVideoDTO dto)
        {
            var video = _mapper.Map<Video>(dto);

            await _unitOfWork.Videos.AddAsync(video);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<VideoDTO>(video);
        }

        public async Task<bool> UpdateAsync(int id, UpdateVideoDTO dto)
        {
            var video = await _unitOfWork.Videos.GetByIdAsync(id);
            if (video == null) return false;

            _mapper.Map(dto, video);
            _unitOfWork.Videos.Update(video);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var video = await _unitOfWork.Videos.GetByIdAsync(id);
            if (video == null) return false;

            _unitOfWork.Videos.Delete(video);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<VideoActivityDTO>> GetVideoActivitiesByIdAsync(int videoId)
        {
            var activities = await _unitOfWork.VideoActivitiesRepo.GetByVideoIdWithDetailsAsync(videoId);
            return _mapper.Map<IEnumerable<VideoActivityDTO>>(activities);
        }

        public async Task<IEnumerable<VideoDTO>> GetVideosByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Enumerable.Empty<VideoDTO>();

            var videos = await _unitOfWork.Videos.GetAllAsync();

            var filtered = videos
                .Where(v => v.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return _mapper.Map<IEnumerable<VideoDTO>>(filtered);
        }

        public async Task<IEnumerable<VideoDTO>> GetTopWatchedVideosAsync(int topCount = 5)
        {
            if (topCount <= 0) topCount = 5;
            if (topCount > 20) topCount = 20;

            var videos = await _unitOfWork.Videos.GetTopWatchedAsync(topCount);
            return _mapper.Map<IEnumerable<VideoDTO>>(videos);
        }
    }
}