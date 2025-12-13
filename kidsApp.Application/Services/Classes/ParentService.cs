using AutoMapper;
using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

public class ParentService : IParentService
{
    private readonly IParentRepository _repo;
    private readonly IMapper _mapper;

    public ParentService(IParentRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    // CRUD
    public async Task<IEnumerable<ParentReadDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<ParentReadDto>>(data);
    }

    public async Task<ParentReadDto> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return _mapper.Map<ParentReadDto>(entity);
    }

    public async Task<ParentReadDto> CreateAsync(ParentCreateDto dto)
    {
        var entity = _mapper.Map<Parent>(dto);
        await _repo.AddAsync(entity);
        return _mapper.Map<ParentReadDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateParentDTO dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        _mapper.Map(dto, entity);
        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        await _repo.DeleteAsync(entity);
        return true;
    }

    // Advanced
    public async Task<IEnumerable<ChildSummaryDTO>> GetChildrenSummaryAsync(int parentId)
    {
        var parent = await _repo.GetByIdAsync(parentId);
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
        var parent = await _repo.GetByIdAsync(parentId);
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
        var parent = (await _repo.GetAllAsync())
                     .FirstOrDefault(p => p.Email == email && p.Password == password);

        if (parent == null) return null;

        // Return JWT token here (for demo we return placeholder)
        return "fake-jwt-token-for-demo";
    }
}
