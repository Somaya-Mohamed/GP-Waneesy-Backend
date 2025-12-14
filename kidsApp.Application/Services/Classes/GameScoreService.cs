using AutoMapper;
using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

public class GameScoreService : IGameScoreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GameScoreService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // CRUD
    public async Task<IEnumerable<GameScoreDTO>> GetAllAsync()
    {
        var data = await _unitOfWork.GameScores.GetAllAsync();
        return _mapper.Map<IEnumerable<GameScoreDTO>>(data);
    }


    public async Task<GameScoreDTO> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.GameScores.GetByIdAsync(id);
        return _mapper.Map<GameScoreDTO>(entity);
    }

    public async Task<GameScoreDTO> CreateAsync(GameScoreCreateDTO dto)
    {
        var entity = _mapper.Map<GameScore>(dto);
        await _unitOfWork.GameScores.AddAsync(entity);
        return _mapper.Map<GameScoreDTO>(entity);
    }

    public async Task<bool> UpdateAsync(int id, GameScoreUpdateDTO dto)
    {
        var entity = await _unitOfWork.GameScores.GetByIdAsync(id);
        if (entity == null) return false;

        _mapper.Map(dto, entity);
        _unitOfWork.GameScores.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.GameScores.GetByIdAsync(id);
        if (entity == null) return false;

        _unitOfWork.GameScores.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    // Advanced Methods
    public async Task<IEnumerable<GameScoreDTO>> GetScoresByGameIdAsync(int gameId)
    {
        var scores = (await _unitOfWork.GameScores.GetAllAsync()).Where(s => s.GameId == gameId);
        return _mapper.Map<IEnumerable<GameScoreDTO>>(scores);
    }

    public async Task<IEnumerable<GameScoreDTO>> GetTopScoresAsync(int topCount)
    {
        var scores = (await _unitOfWork.GameScores.GetAllAsync())
            .OrderByDescending(s => s.ScoreValue)
            .Take(topCount);
        return _mapper.Map<IEnumerable<GameScoreDTO>>(scores);
    }
}
