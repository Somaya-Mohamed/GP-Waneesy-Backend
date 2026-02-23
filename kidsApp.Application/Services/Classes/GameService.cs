using AutoMapper;
using kidsApp.Application.DTOs.GameDTOs;
using kidsApp.Application.ServiceManager;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System.Runtime.InteropServices;

public class GameService : IGameService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GameService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // CRUD
    public async Task<IEnumerable<GameReadDto>> GetAllAsync()
    {
        var data = await _unitOfWork.Games.GetAllAsync();
        return _mapper.Map<IEnumerable<GameReadDto>>(data);
    }

    public async Task<GameReadDto> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.Games.GetByIdAsync(id);
        return _mapper.Map<GameReadDto>(entity);
    }

    public async Task<GameReadDto> CreateAsync(GameCreateDTO dto)
    {
        var entity = _mapper.Map<Game>(dto);
        await _unitOfWork.Games.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<GameReadDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, GameUpdateDTO dto)
    {
        var entity = await _unitOfWork.Games.GetByIdAsync(id);
        if (entity == null) return false;

        _mapper.Map(dto, entity);
        _unitOfWork.Games.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Games.GetByIdAsync(id);
        if (entity == null) return false;

        _unitOfWork.Games.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    // Advanced Methods
    public async Task<IEnumerable<GameReadDto>> GetGamesByDifficultyAsync(string level)
    {
        var data = (await _unitOfWork.Games.GetAllAsync()).Where(g => g.DifficultyLevel == level);
        return _mapper.Map<IEnumerable<GameReadDto>>(data);
    }
}
