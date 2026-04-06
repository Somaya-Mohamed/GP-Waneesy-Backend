using AutoMapper;
using kidsApp.Application.DTOs.VideoDTOs;
using kidsApp.Application.DTOs.VideoActivityDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class VideoService : IVideoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly string _baseVideoUrl = "https://myvideoserver.com/videos/"; // رابط ثابت للفيديوهات

    public VideoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VideoDTO>> GetAllAsync()
    {
        var videos = await _unitOfWork.Videos.GetAllAsync();
        var videoDTOs = _mapper.Map<IEnumerable<VideoDTO>>(videos)
            .Select(v =>
            {
                v.Url = $"{_baseVideoUrl}{v.Id}.mp4"; // نضيف URL جاهز
                return v;
            });
        return videoDTOs;
    }

    public async Task<VideoDTO> GetByIdAsync(int id)
    {
        var video = await _unitOfWork.Videos.GetByIdAsync(id);
        if (video == null) return null;

        var videoDTO = _mapper.Map<VideoDTO>(video);
        videoDTO.Url = $"{_baseVideoUrl}{video.Id}.mp4"; // نضيف URL للفيديو الفردي
        return videoDTO;
    }

    public async Task<VideoDTO> CreateAsync(CreateVideoDTO dto)
    {
        var video = _mapper.Map<Video>(dto);

        await _unitOfWork.Videos.AddAsync(video);
        await _unitOfWork.SaveChangesAsync();

        var videoDTO = _mapper.Map<VideoDTO>(video);
        videoDTO.Url = $"{_baseVideoUrl}{video.Id}.mp4";
        return videoDTO;
    }

    public async Task<bool> UpdateAsync(int id, UpdateVideoDTO dto)
    {
        var video = await _unitOfWork.Videos.GetByIdAsync(id);
        if (video == null) return false;

        _mapper.Map(dto, video);
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

    public async Task<IEnumerable<VideoDTO>> GetVideosByCategoryAsync(string level)
    {
        var videos = await _unitOfWork.Videos.GetByCategoryAsync(level);
        var videoDTOs = _mapper.Map<IEnumerable<VideoDTO>>(videos)
            .Select(v =>
            {
                v.Url = $"{_baseVideoUrl}{v.Id}.mp4";
                return v;
            });
        return videoDTOs;
    }

    public async Task<IEnumerable<VideoDTO>> GetTopWatchedVideosAsync(int topCount = 5)
    {
        var videos = await _unitOfWork.Videos.GetTopWatchedAsync(topCount);
        var videoDTOs = _mapper.Map<IEnumerable<VideoDTO>>(videos)
            .Select(v =>
            {
                v.Url = $"{_baseVideoUrl}{v.Id}.mp4";
                return v;
            });
        return videoDTOs;
    }
}