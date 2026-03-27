using AutoMapper;
using kidsApp.Application.DTOs.ActivityDTOs;
using kidsApp.Application.DTOs.ChildDTOs;
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
    public class ChildService : IChildService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChildService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ====================== Basic CRUD ======================
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

        // ====================== Advanced Methods ======================

        public async Task<IEnumerable<ProgressReadDto>> GetWeeklyProgressAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdWithDetailsAsync(childId);
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
                        Title = p.Story?.Title ?? "Unknown Story"
                    },
                    Score = 0,
                    DateCompleted = p.LastUpdated
                });

            return progresses;
        }

        // Total Points
        public async Task<int> GetTotalPointsAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdWithDetailsAsync(childId);
            if (child == null) return 0;

            var gamePoints = child.GameScores?.Sum(g => g.ScoreValue) ?? 0;
            var taskPoints = child.TaskLogs?.Sum(t => t.PointsEarned) ?? 0;
            var storyPoints = child.StoryProgress?
                .Where(sp => sp.ProgressPercent >= 100)
                .Sum(sp => sp.Story?.PointsRewarded ?? 0) ?? 0;

            return gamePoints + taskPoints + storyPoints;
        }

        public async Task<double> GetCompletionPercentageAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdWithDetailsAsync(childId);
            if (child == null) return 0;

            int totalActivities = (child.StoryProgress?.Count ?? 0)
                                + (child.VideoActivities?.Count ?? 0)
                                + (child.TaskLogs?.Count ?? 0)
                                + (child.GameScores?.Count ?? 0);

            int completedActivities = (child.StoryProgress?.Count(sp => sp.ProgressPercent >= 100) ?? 0)
                                    + (child.VideoActivities?.Count(v => v.WatchedPercent >= 100) ?? 0)
                                    + (child.TaskLogs?.Count(t => t.Status == "Completed") ?? 0)
                                    + (child.GameScores?.Count(g => g.ScoreValue > 0) ?? 0);

            if (totalActivities == 0) return 0;

            return Math.Round((double)completedActivities / totalActivities * 100, 2);
        }

        // Weekly Report 
        public async Task<ChildReportDTO> GetWeeklyReportAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdWithDetailsAsync(childId);
            if (child == null) return null;

            var lastWeek = DateTime.UtcNow.AddDays(-7);

            var weeklyGames = child.GameScores?.Where(g => g.Date >= lastWeek).ToList() ?? new List<GameScore>();
            var weeklyStories = child.StoryProgress?.Where(s => s.LastUpdated >= lastWeek).ToList() ?? new List<StoryProgress>();
            var weeklyTasks = child.TaskLogs?.Where(t => t.DateCompleted >= lastWeek).ToList() ?? new List<TaskLog>();
            var weeklyVideos = child.VideoActivities?.Where(v => v.LastUpdated >= lastWeek).ToList() ?? new List<VideoActivity>();

            var report = new ChildReportDTO
            {
                ChildId = child.Id,
                FullName = child.Name,
                TotalPoints = weeklyGames.Sum(g => g.ScoreValue) +
                             weeklyTasks.Sum(t => t.PointsEarned) +
                             weeklyStories.Where(s => s.ProgressPercent >= 100)
                                         .Sum(s => s.Story?.PointsRewarded ?? 0),
                GamesPlayed = weeklyGames.Count,
                StoriesCompleted = weeklyStories.Count(s => s.ProgressPercent >= 100),
                TasksCompleted = weeklyTasks.Count(t => t.Status == "Completed"),
                VideosCompleted = weeklyVideos.Count(v => v.WatchedPercent >= 100)
            };

            return report;
        }

        // Activities Summary 
        public async Task<ChildActivitiesSummaryDTO> GetChildActivitiesSummaryAsync(int childId)
        {
            var child = await _unitOfWork.Children.GetByIdWithDetailsAsync(childId);
            if (child == null) return null;

            var summary = new ChildActivitiesSummaryDTO
            {
                ChildId = child.Id,
                FullName = child.Name,

                TotalStories = child.StoryProgress?.Count ?? 0,
                CompletedStories = child.StoryProgress?.Count(sp => sp.ProgressPercent >= 100) ?? 0,

                TotalVideos = child.VideoActivities?.Count ?? 0,
                CompletedVideos = child.VideoActivities?.Count(v => v.WatchedPercent >= 100) ?? 0,

                TotalTasks = child.TaskLogs?.Count ?? 0,
                CompletedTasks = child.TaskLogs?.Count(t => t.Status == "Completed") ?? 0,

                TotalGames = child.GameScores?.Count ?? 0,
                CompletedGames = child.GameScores?.Count(g => g.ScoreValue > 0) ?? 0
            };

            summary.TotalActivities = summary.TotalStories + summary.TotalVideos +
                                      summary.TotalTasks + summary.TotalGames;

            summary.CompletedActivities = summary.CompletedStories + summary.CompletedVideos +
                                          summary.CompletedTasks + summary.CompletedGames;

            summary.CompletionPercentage = summary.TotalActivities == 0
                ? 0
                : Math.Round((double)summary.CompletedActivities / summary.TotalActivities * 100, 2);

            // Recent Activities
            var recent = new List<RecentActivityDto>();

            recent.AddRange(child.StoryProgress?
                .OrderByDescending(s => s.LastUpdated)
                .Take(2)
                .Select(s => new RecentActivityDto
                {
                    ActivityType = "Story",
                    Title = s.Story?.Title ?? "Unknown Story",
                    Progress = s.ProgressPercent,
                    Date = s.LastUpdated
                }) ?? Enumerable.Empty<RecentActivityDto>());

            recent.AddRange(child.VideoActivities?
                .OrderByDescending(v => v.LastUpdated ?? DateTime.UtcNow)
                .Take(2)
                .Select(v => new RecentActivityDto
                {
                    ActivityType = "Video",
                    Title = v.Video?.Title ?? "Unknown Video",
                    Progress = v.WatchedPercent,
                    Date = v.LastUpdated ?? DateTime.UtcNow
                }) ?? Enumerable.Empty<RecentActivityDto>());

            recent.AddRange(child.GameScores?
                .OrderByDescending(g => g.Date)
                .Take(2)
                .Select(g => new RecentActivityDto
                {
                    ActivityType = "Game",
                    Title = g.Game?.Title ?? "Unknown Game",
                    Progress = 100,
                    Date = g.Date
                }) ?? Enumerable.Empty<RecentActivityDto>());

            recent.AddRange(child.TaskLogs?
                .OrderByDescending(t => t.DateCompleted)
                .Take(1)
                .Select(t => new RecentActivityDto
                {
                    ActivityType = "Task",
                    Title = t.Task?.Title ?? "Unknown Task",
                    Progress = t.Status == "Completed" ? 100 : 0,
                    Date = t.DateCompleted ?? DateTime.UtcNow
                }) ?? Enumerable.Empty<RecentActivityDto>());

            summary.RecentActivities = recent
                .OrderByDescending(r => r.Date)
                .Take(5)
                .ToList();

            return summary;
        }

        public async Task<IEnumerable<ChildTopScoreDTO>> GetTopScoresAsync(int childId, int topCount = 5)
        {
            var child = await _unitOfWork.Children.GetByIdWithDetailsAsync(childId);
            if (child == null) return Enumerable.Empty<ChildTopScoreDTO>();

            var topScores = child.GameScores?
                .OrderByDescending(g => g.ScoreValue)
                .Take(topCount)
                .Select(g => new ChildTopScoreDTO
                {
                    GameId = g.GameId,
                    GameTitle = g.Game?.Title ?? "Unknown Game",
                    Score = g.ScoreValue,
                    Date = g.Date
                }) ?? Enumerable.Empty<ChildTopScoreDTO>();

            return topScores;
        }
    }
}