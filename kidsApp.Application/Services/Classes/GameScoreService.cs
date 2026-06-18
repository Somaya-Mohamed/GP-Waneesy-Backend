using AutoMapper;
using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
{
    public class GameScoreService : IGameScoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameScoreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameScoreDTO>> GetAllAsync()
        {
            var scores = await _unitOfWork.GameScores.GetWithDetailsAsync();
            return _mapper.Map<IEnumerable<GameScoreDTO>>(scores);
        }

        public async Task<GameScoreDTO?> GetByIdAsync(int id)
        {
            var scores = await _unitOfWork.GameScores.GetWithDetailsAsync();
            var entity = scores.FirstOrDefault(gs => gs.Id == id);
            return entity == null ? null : _mapper.Map<GameScoreDTO>(entity);
        }

        public async Task<GameScoreDTO> CreateAsync(GameScoreCreateDTO dto)
        {
            var entity = _mapper.Map<GameScore>(dto);
            entity.Date = DateTime.UtcNow;

            await _unitOfWork.GameScores.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var created = (await _unitOfWork.GameScores.GetWithDetailsAsync())
                .FirstOrDefault(gs => gs.Id == entity.Id);

            return _mapper.Map<GameScoreDTO>(created!);
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

        public async Task<IEnumerable<GameScoreDTO>> GetScoresByGameIdAsync(int gameId)
        {
            var scores = await _unitOfWork.GameScores.GetWithDetailsAsync();
            var filtered = scores
                .Where(gs => gs.GameId == gameId)
                .OrderByDescending(gs => gs.Date)
                .ToList();

            return _mapper.Map<IEnumerable<GameScoreDTO>>(filtered);
        }

        public async Task<IEnumerable<GameScoreDTO>> GetScoresByChildIdAsync(int childId)
        {
            var scores = await _unitOfWork.GameScores.GetWithDetailsAsync();
            var filtered = scores
                .Where(gs => gs.ChildId == childId)
                .OrderByDescending(gs => gs.Date)
                .ToList();

            return _mapper.Map<IEnumerable<GameScoreDTO>>(filtered);
        }

        public async Task<IEnumerable<GameScoreDTO>> GetTopScoresAsync(int topCount)
        {
            if (topCount <= 0) topCount = 10;
            if (topCount > 50) topCount = 50;

            var scores = await _unitOfWork.GameScores.GetWithDetailsAsync();
            var topScores = scores
                .OrderByDescending(gs => gs.ScoreValue)
                .Take(topCount)
                .ToList();

            return _mapper.Map<IEnumerable<GameScoreDTO>>(topScores);
        }

        public async Task<IEnumerable<GameScoreDTO>> GetMyScoresAsync(int childId)
        {
            return await GetScoresByChildIdAsync(childId);
        }
    }
}