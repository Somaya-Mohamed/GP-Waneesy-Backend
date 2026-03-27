using AutoMapper;
using kidsApp.Application.DTOs.GameDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ====================== CRUD ======================
        public async Task<IEnumerable<GameReadDto>> GetAllAsync()
        {
            var games = await _unitOfWork.Games.GetAllAsync();
            return _mapper.Map<IEnumerable<GameReadDto>>(games);
        }

        public async Task<GameReadDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Games.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GameReadDto>(entity);
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

        // ====================== Advanced Methods ======================
        public async Task<IEnumerable<GameReadDto>> GetGamesByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Enumerable.Empty<GameReadDto>();

            var games = await _unitOfWork.Games.GetAllAsync();

            var filtered = games
                .Where(g => g.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return _mapper.Map<IEnumerable<GameReadDto>>(filtered);
        }

        public async Task<IEnumerable<GameReadDto>> GetGamesByDifficultyAsync(string difficulty)
        {
            if (string.IsNullOrWhiteSpace(difficulty))
                return Enumerable.Empty<GameReadDto>();

            var games = await _unitOfWork.Games.GetAllAsync();

            var filtered = games
                .Where(g => g.DifficultyLevel.Equals(difficulty, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return _mapper.Map<IEnumerable<GameReadDto>>(filtered);
        }
    }
}