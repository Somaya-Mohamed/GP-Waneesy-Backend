using AutoMapper;
using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

public class ParentService : IParentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ParentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    // CRUD
    public async Task<IEnumerable<ParentReadDto>> GetAllAsync()
    {
        var data = await _unitOfWork.Parents.GetAllAsync();
        return _mapper.Map<IEnumerable<ParentReadDto>>(data);
    }

    public async Task<ParentReadDto> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.Parents.GetByIdAsync(id);
        return _mapper.Map<ParentReadDto>(entity);
    }

    public async Task<ParentReadDto> CreateAsync(ParentCreateDto dto)
    {
        var entity = _mapper.Map<Parent>(dto);
        await _unitOfWork.Parents.AddAsync(entity);
        return _mapper.Map<ParentReadDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateParentDTO dto)
    {
        var entity = await _unitOfWork.Parents.GetByIdAsync(id);
        if (entity == null) return false;

        _mapper.Map(dto, entity);
        _unitOfWork.Parents.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Parents.GetByIdAsync(id);
        if (entity == null) return false;

        _unitOfWork.Parents.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    // Advanced
    public async Task<IEnumerable<ChildSummaryDTO>> GetChildrenSummaryAsync(int parentId)
    {
        var parent = await _unitOfWork.Parents.GetByIdAsync(parentId);
        if (parent == null) return Enumerable.Empty<ChildSummaryDTO>();

        return parent.Children?.Select(c => new ChildSummaryDTO
        {
            ChildId = c.Id,
            FullName = c.Name,
            Age = c.Age
        }) ?? Enumerable.Empty<ChildSummaryDTO>();
    }

    public async Task<IEnumerable<ProgressReadDto>> GetWeeklyChildReportsAsync(int parentId)
    {
        var parent = await _unitOfWork.Parents.GetByIdAsync(parentId);
        if (parent == null) return Enumerable.Empty<ProgressReadDto>();

        var lastWeek = DateTime.UtcNow.AddDays(-7);

        var reports = parent.Children?
            .SelectMany(c => c.StoryProgress
                .Where(p => p.DateCompleted >= lastWeek)
                .Select(p => _mapper.Map<ProgressReadDto>(p)))
            ?? Enumerable.Empty<ProgressReadDto>();

        return reports;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var parent = (await _unitOfWork.Parents.GetAllAsync())
                     .FirstOrDefault(p => p.Email == email && p.Password == password);

        if (parent == null) return null;

        // Return JWT token here (for demo we return placeholder)
        return "fake-jwt-token-for-demo";
    }
}
