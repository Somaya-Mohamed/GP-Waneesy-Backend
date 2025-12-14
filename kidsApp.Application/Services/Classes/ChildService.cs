using AutoMapper;
using kidsApp.Application.DTOs.ActivityDTOs;
using kidsApp.Application.DTOs.ChildDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

namespace kidsApp.Application.Services
{
    public class ChildService : IChildService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChildService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChildReadDTO>> GetAllAsync()
        {
            var children = await _unitOfWork.Children.GetAllAsync();
            return _mapper.Map<IEnumerable<ChildReadDTO>>(children);
        }

        public async Task<ChildReadDTO?> GetByIdAsync(int id)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(id);
            return child == null ? null : _mapper.Map<ChildReadDTO>(child);
        }

        public async Task<ChildReadDTO> CreateAsync(ChildCreateDTO dto)
        {
            var entity = _mapper.Map<Child>(dto);
            await _unitOfWork.Children.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ChildReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ChildUpdateDto dto)
        {
            var entity = await _unitOfWork.Children.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            _unitOfWork.Children.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Children.GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.Children.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // ------------------ Advanced Methods ------------------

        // Weekly progress of the child
        public async Task<IEnumerable<ProgressReadDto>> GetWeeklyProgressAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(childId);
            if (child == null) return Enumerable.Empty<ProgressReadDto>();

            var lastWeek = DateTime.UtcNow.AddDays(-7);

            var progresses = child.StoryProgress
                .Where(p => p.LastUpdated >= lastWeek)
                .Select(p => new ProgressReadDto
                {
                    Id = p.Id,
                    Kid = _mapper.Map<ChildReadDTO>(p.Child),
                    Activity = new ActivityReadDto
                    {
                        Id = p.StoryId,
                        Title = p.Story.Title
                    },
                    Score = 0, // Compute if needed
                    DateCompleted = p.LastUpdated
                });

            return progresses;
        }

        // Total points earned by the child
        public async Task<int> GetTotalPointsAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(childId);
            if (child == null) return 0;

            var gamePoints = child.GameScores?.Sum(g => g.ScoreValue) ?? 0;
            var taskPoints = child.TaskLogs?.Sum(t => t.PointsEarned) ?? 0;

            return gamePoints + taskPoints;
        }

        // Completion percentage across all activities
        public async Task<double> GetCompletionPercentageAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(childId);
            if (child == null) return 0;

            int totalActivities = (child.StoryProgress?.Count ?? 0)
                                + (child.VideoActivities?.Count ?? 0)
                                + (child.TaskLogs?.Count ?? 0);

            int completedActivities = (child.StoryProgress?.Count(sp => sp.ProgressPercent >= 100) ?? 0)
                                    + (child.VideoActivities?.Count(v => v.WatchedPercent >= 100) ?? 0)
                                    + (child.TaskLogs?.Count(t => t.Status == "Completed") ?? 0);

            if (totalActivities == 0) return 0;

            return Math.Round((double)completedActivities / totalActivities * 100, 2);
        }

        // Weekly report for child (combining scores, progress, tasks)
        public async Task<ChildReportDTO> GetWeeklyReportAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(childId);
            if (child == null) return null;

            var lastWeek = DateTime.UtcNow.AddDays(-7);

            var weeklyGames = child.GameScores?.Where(g => g.Date >= lastWeek).ToList() ?? new List<GameScore>();
            var weeklyStories = child.StoryProgress?.Where(s => s.LastUpdated >= lastWeek).ToList() ?? new List<StoryProgress>();
            var weeklyTasks = child.TaskLogs?.Where(t => t.DateCompleted >= lastWeek).ToList() ?? new List<TaskLog>();

            var report = new ChildReportDTO
            {
                ChildId = child.Id,
                FullName = child.Name,
                TotalPoints = (weeklyGames.Sum(g => g.ScoreValue) + weeklyTasks.Sum(t => t.PointsEarned)),
                GamesPlayed = weeklyGames.Count,
                StoriesCompleted = weeklyStories.Count(s => s.ProgressPercent >= 100),
                TasksCompleted = weeklyTasks.Count(t => t.Status == "Completed")
            };

            return report;
        }

        // Summary of child activities
        public async Task<IEnumerable<ProgressReadDto>> GetChildActivitiesSummaryAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(childId);
            if (child == null) return Enumerable.Empty<ProgressReadDto>();

            var allActivities = child.StoryProgress.Select(sp => new ProgressReadDto
            {
                Id = sp.Id,
                Kid = _mapper.Map<ChildReadDTO>(sp.Child),
                Activity = new ActivityReadDto
                {
                    Id = sp.StoryId,
                    Title = sp.Story.Title
                },
                Score = 0,
                DateCompleted = sp.LastUpdated
            });

            return allActivities;
        }

        // Top scores of child
        public async Task<IEnumerable<ChildTopScoreDTO>> GetTopScoresAsync(int childId, int topCount = 5)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(childId);
            if (child == null) return Enumerable.Empty<ChildTopScoreDTO>();

            var topScores = child.GameScores?
                .OrderByDescending(g => g.ScoreValue)
                .Take(topCount)
                .Select(g => new ChildTopScoreDTO
                {
                    GameId = g.GameId,
                    GameTitle = g.Game.Title,
                    Score = g.ScoreValue,
                    Date = g.Date
                }) ?? Enumerable.Empty<ChildTopScoreDTO>();

            return topScores;
        }
    }
}
