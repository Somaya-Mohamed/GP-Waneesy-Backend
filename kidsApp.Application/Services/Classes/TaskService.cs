using AutoMapper;
using kidsApp.Application.DTOs.TaskDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskDTO>> GetAllAsync()
        {
            var tasks = await _unitOfWork.Tasks.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
        }

        public async Task<TaskDTO?> GetByIdAsync(int id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            return task == null ? null : _mapper.Map<TaskDTO>(task);
        }

        public async Task<TaskDTO> CreateAsync(CreateTaskDTO dto)
        {
            var task = _mapper.Map<Tasks>(dto);

            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaskDTO>(task);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTaskDTO dto)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null) return false;

            _mapper.Map(dto, task);
            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null) return false;

            _unitOfWork.Tasks.Delete(task);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<TaskDTO>> GetTasksByDifficultyAsync(string difficulty)
        {
            if (string.IsNullOrWhiteSpace(difficulty))
                return Enumerable.Empty<TaskDTO>();

            var tasks = await _unitOfWork.Tasks.GetAllAsync();

            var filtered = tasks
                .Where(t => t.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return _mapper.Map<IEnumerable<TaskDTO>>(filtered);
        }
    }
}