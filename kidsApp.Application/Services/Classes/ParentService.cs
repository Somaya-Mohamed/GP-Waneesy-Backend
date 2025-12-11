using AutoMapper;
using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain;
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
        _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        _repo.DeleteAsync(entity);
        return true;
    }
}
