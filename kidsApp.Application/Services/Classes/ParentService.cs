using AutoMapper;
using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
{
    public class ParentService : IParentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParentReadDto>> GetAllAsync()
        {
            var parents = await _unitOfWork.Parents.GetAllWithChildrenAsync();   
            return _mapper.Map<IEnumerable<ParentReadDto>>(parents);
        }

        public async Task<ParentReadDto?> GetByIdAsync(int id)
        {
            var parent = await _unitOfWork.Parents.GetParentWithChildren(id);
            return parent == null ? null : _mapper.Map<ParentReadDto>(parent);
        }

        public async Task<ParentReadDto> CreateAsync(ParentCreateDto dto)
        {
            var parent = _mapper.Map<Parent>(dto);

            await _unitOfWork.Parents.AddAsync(parent);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ParentReadDto>(parent);
        }

        public async Task<bool> UpdateAsync(int id, UpdateParentDTO dto)
        {
            var parent = await _unitOfWork.Parents.GetParentWithChildren(id);
            if (parent == null) return false;

            _mapper.Map(dto, parent);
            _unitOfWork.Parents.Update(parent);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var parent = await _unitOfWork.Parents.GetParentWithChildren(id);
            if (parent == null) return false;

            _unitOfWork.Parents.Delete(parent);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ChildSummaryDTO>> GetChildrenSummaryAsync(int parentId)
        {
            var parent = await _unitOfWork.Parents.GetParentWithChildren(parentId);
            if (parent == null || parent.Children == null)
                return Enumerable.Empty<ChildSummaryDTO>();

            return _mapper.Map<IEnumerable<ChildSummaryDTO>>(parent.Children);
        }

        public async Task<IEnumerable<ProgressReadDto>> GetWeeklyChildReportsAsync(int parentId)
        {
            var parent = await _unitOfWork.Parents.GetParentWithChildren(parentId);
            if (parent == null || parent.Children == null)
                return Enumerable.Empty<ProgressReadDto>();

            var lastWeek = DateTime.UtcNow.AddDays(-7);

            var reports = parent.Children
                .SelectMany(c => c.StoryProgress?.Where(p =>
                    p.LastUpdated >= lastWeek) ?? Enumerable.Empty<StoryProgress>())
                .Select(p => _mapper.Map<ProgressReadDto>(p))
                .ToList();

            return reports;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var parent = (await _unitOfWork.Parents.GetAllAsync())
                .FirstOrDefault(p => p.Email == email && p.Password == password);

            if (parent == null) return null;

            // TODO: في المستقبل هترجع JWT Token حقيقي
            return "demo-jwt-token-for-parent-" + parent.Id;
        }
    }
}