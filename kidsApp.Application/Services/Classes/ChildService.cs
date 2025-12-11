using AutoMapper;
using kidsApp.Application.DTOs.ChildDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

public class ChildService : IChildService
{
    private readonly IChildRepository _repo;
    private readonly IMapper _mapper;

    public ChildService(IChildRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ChildReadDTO>> GetAllAsync() // Updated return type to match IChildService
    {
        var data = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<ChildReadDTO>>(data); // Updated mapping type
    }

    public async Task<ChildReadDTO> GetByIdAsync(int id) // Updated return type to match IChildService
    {
        var entity = await _repo.GetByIdAsync(id);
        return _mapper.Map<ChildReadDTO>(entity); // Updated mapping type
    }

    public async Task<ChildReadDTO> CreateAsync(ChildCreateDTO dto) // Updated parameter and return types to match IChildService
    {
        var entity = _mapper.Map<Child>(dto);
        await _repo.AddAsync(entity);
        return _mapper.Map<ChildReadDTO>(entity); // Updated mapping type
    }

    public async Task<bool> UpdateAsync(int id, ChildUpdateDto dto) // Updated parameter type to match IChildService
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        _mapper.Map(dto, entity);
        _repo.UpdateAsync(entity); // Fixed method call to match IGenericRepository
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        _repo.DeleteAsync(entity); // Fixed method call to match IGenericRepository
        return true;
    }
}
