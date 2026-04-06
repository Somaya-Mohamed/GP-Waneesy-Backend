using AutoMapper;
using kidsApp.Application.DTOs.VideoActivityDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

public class VideoActivityService : IVideoActivityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VideoActivityService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VideoActivityDTO>> GetAllAsync()
    {
        var activities = await _unitOfWork.VideoActivitiesRepo.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<VideoActivityDTO>>(activities);
    }

    public async Task<VideoActivityDTO> GetByIdAsync(int id)
    {
        var activity = await _unitOfWork.VideoActivitiesRepo.GetByIdWithDetailsAsync(id);
        return activity == null ? null : _mapper.Map<VideoActivityDTO>(activity);
    }

    public async Task<VideoActivityDTO> CreateAsync(CreateVideoActivityDTO dto)
    {
        // Validate Child  
        var child = await _unitOfWork.Children.GetByIdAsync(dto.ChildId);
        if (child == null)
            throw new Exception($"Child with ID {dto.ChildId} does not exist.");

        // Validate Video  
        var video = await _unitOfWork.Videos.GetByIdAsync(dto.VideoId);
        if (video == null)
            throw new Exception($"Video with ID {dto.VideoId} does not exist.");

        // Map DTO to entity  
        var entity = _mapper.Map<VideoActivity>(dto);

        // Set defaults  
        entity.Status = "Started";
        entity.WatchedPercent = 0;

        // Add to DB  
        await _unitOfWork.VideoActivitiesRepo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        // Reload with navigation properties  
        var created = await _unitOfWork.VideoActivitiesRepo.GetByIdWithDetailsAsync(entity.Id);

        // Map to DTO  
        return _mapper.Map<VideoActivityDTO>(created);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var activity = await _unitOfWork.VideoActivitiesRepo.GetByIdAsync(id);
        if (activity == null) return false;

        _unitOfWork.VideoActivitiesRepo.Delete(activity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProgressAsync(int id, double watchPercent, string status)
    {
        var activity = await _unitOfWork.VideoActivitiesRepo.GetByIdAsync(id);
        if (activity == null) return false;

        activity.WatchedPercent = watchPercent;
        activity.Status = status;

        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<VideoActivityDTO>> GetByChildIdAsync(int childId)
    {
        var activities = await _unitOfWork.VideoActivitiesRepo.GetByChildIdWithDetailsAsync(childId);
        return _mapper.Map<IEnumerable<VideoActivityDTO>>(activities);
    }

    public async Task<IEnumerable<VideoActivityDTO>> GetProgressByVideoIdAsync(int videoId)
    {
        var activities = await _unitOfWork.VideoActivitiesRepo.GetByVideoIdWithDetailsAsync(videoId);
        return _mapper.Map<IEnumerable<VideoActivityDTO>>(activities);
    }
}