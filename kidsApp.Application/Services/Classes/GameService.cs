using AutoMapper;
using kidsApp.Application.DTOs.GameDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

public class GameService : IGameService
{
    private readonly IGenericRepository<Game> _repo;
    private readonly IMapper _mapper;

    public GameService(IGenericRepository<Game> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    // CRUD
    public async Task<IEnumerable<GameReadDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<GameReadDto>>(data);
    }

    public async Task<GameReadDto> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return _mapper.Map<GameReadDto>(entity);
    }

    public async Task<GameReadDto> CreateAsync(GameCreateDTO dto)
    {
        var entity = _mapper.Map<Game>(dto);
        await _repo.AddAsync(entity);
        return _mapper.Map<GameReadDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, GameUpdateDTO dto)
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

    // Advanced Methods
    public async Task<IEnumerable<GameReadDto>> GetGamesByDifficultyAsync(string level)
    {
        var data = (await _repo.GetAllAsync()).Where(g => g.DifficultyLevel == level);
        return _mapper.Map<IEnumerable<GameReadDto>>(data);
    }
}
