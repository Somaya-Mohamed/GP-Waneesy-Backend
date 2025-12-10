using AutoMapper;
using kidsApp.Application.Dto.GameDTOs;
using kidsApp.Application.Dto.GameScoreDTOs;
using kidsApp.Application.Interfaces;
using kidsApp.Application.Interfaces.Repository;
using kidsApp.Application.Services;
using kidsApp.Domain;
using kidsApp.Domain.Entites;

namespace kidsApp.Infrastructure.Services
{
    public class GameScoreService : IGameScoreService
    {
        private readonly IGameScoreRepository _repo;
        private readonly IMapper _mapper;

        public GameScoreService(IGameScoreRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

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
    }


}
