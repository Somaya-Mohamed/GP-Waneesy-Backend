using AutoMapper;
using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

public class GameScoreService : IGameScoreService
{
    private readonly IGenericRepository<GameScore> _repo;
    private readonly IMapper _mapper;

    public GameScoreService(IGenericRepository<GameScore> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    // CRUD
    public async Task<IEnumerable<GameScoreDTO>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<GameScoreDTO>>(data);
    }

    public async Task<GameScoreDTO> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return _mapper.Map<GameScoreDTO>(entity);
    }

    public async Task<GameScoreDTO> CreateAsync(GameScoreCreateDTO dto)
    {
        var entity = _mapper.Map<GameScore>(dto);
        await _repo.AddAsync(entity);
        return _mapper.Map<GameScoreDTO>(entity);
    }

    public async Task<bool> UpdateAsync(int id, GameScoreUpdateDTO dto)
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
    public async Task<IEnumerable<GameScoreDTO>> GetScoresByGameIdAsync(int gameId)
    {
        var scores = (await _repo.GetAllAsync()).Where(s => s.GameId == gameId);
        return _mapper.Map<IEnumerable<GameScoreDTO>>(scores);
    }

    public async Task<IEnumerable<GameScoreDTO>> GetTopScoresAsync(int topCount)
    {
        var scores = (await _repo.GetAllAsync())
            .OrderByDescending(s => s.ScoreValue)
            .Take(topCount);
        return _mapper.Map<IEnumerable<GameScoreDTO>>(scores);
    }
}
