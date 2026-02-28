using AutoMapper;
using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Classes
{
    public class TaskLogService : ITaskLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskLogDTO>> GetAllAsync()
        {
            var logs = await _unitOfWork.TaskLogs.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskLogDTO>>(logs);
        }

        public async Task<TaskLogDTO> GetByIdAsync(int id)
        {
            var log = await _unitOfWork.TaskLogs.GetByIdAsync(id);
            return _mapper.Map<TaskLogDTO>(log);
        }
        // this to solve problem why childid,log=null in tasklog table when create new log
        //public async Task<TaskLogDTO> CreateAsync(CreateTaskLogDTO dto)
        //{
        //    var log = _mapper.Map<TaskLog>(dto);
        //    log.Status = dto.IsCompleted ? "Completed" : "Pending";
        //    log.DateCompleted = dto.IsCompleted ? DateTime.UtcNow : default;

        //    await _unitOfWork.TaskLogs.AddAsync(log);
        //    await _unitOfWork.SaveChangesAsync();

        //    // Reload including navigation
        //    var createdLog = await (_unitOfWork.TaskLogs as TaskLogRepository)
        //        .GetByIdWithRelationsAsync(log.Id);

        //    return _mapper.Map<TaskLogDTO>(createdLog);
        //}
        public async Task<TaskLogDTO> CreateAsync(CreateTaskLogDTO dto)
        {
            var log = _mapper.Map<TaskLog>(dto);
            log.Status = dto.IsCompleted ? "Completed" : "Pending";
            log.DateCompleted = dto.IsCompleted ? DateTime.UtcNow : default;

            await _unitOfWork.TaskLogs.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();

            // Removed the reference to TaskLogRepository as it is not defined in the provided context.
            // Assuming GetByIdAsync is sufficient to retrieve the created entity.
            var createdLog = await _unitOfWork.TaskLogs.GetByIdAsync(log.Id);

            return _mapper.Map<TaskLogDTO>(createdLog);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var log = await _unitOfWork.TaskLogs.GetByIdAsync(id);
            if (log == null) return false;

            _unitOfWork.TaskLogs.Delete(log);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}