using AutoMapper;
using kidsApp.Application.Dto.GameDTOs;
using kidsApp.Application.Interfaces;
using kidsApp.Application.Interfaces.Repository;
using kidsApp.Application.Services;
using kidsApp.Domain;
using kidsApp.Domain.Entites;

public class GameService : IGameService
{
    private readonly IGameRepository _repo;
    private readonly IMapper _mapper;

    public GameService(IGameRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

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
}
