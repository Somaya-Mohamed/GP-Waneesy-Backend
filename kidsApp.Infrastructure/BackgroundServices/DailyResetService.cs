//using kidsApp.Application.Services.Interfaces;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;

//namespace kidsApp.Infrastructure.BackgroundServices
//{

//    public class DailyResetService : BackgroundService
//    {
//        private readonly IServiceScopeFactory _scopeFactory;
//        private readonly ILogger<DailyResetService> _logger;

//        public DailyResetService(IServiceScopeFactory scopeFactory, ILogger<DailyResetService> logger)
//        {
//            _scopeFactory = scopeFactory;
//            _logger = logger;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            _logger.LogInformation("DailyResetService started.");

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                var delay = GetDelayUntilMidnight();
//                _logger.LogInformation("Next daily reset in: {Delay}", delay);

//                await Task.Delay(delay, stoppingToken);

//                if (stoppingToken.IsCancellationRequested)
//                    break;

//                await RunResetAsync();
//            }

//            _logger.LogInformation("DailyResetService stopped.");
//        }

//        private async Task RunResetAsync()
//        {
//            try
//            {
//                _logger.LogInformation("Running daily task-log reset at {Time} UTC", DateTime.UtcNow);

//                // Use a new scope because ITaskLogService is Scoped, not Singleton
//                using var scope = _scopeFactory.CreateScope();
//                var taskLogService = scope.ServiceProvider.GetRequiredService<ITaskLogService>();

//                await taskLogService.ResetDailyLogsAsync();

//                _logger.LogInformation("Daily task-log reset completed successfully.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error during daily task-log reset." ,ex.Message );
//            }
//        }

//        private static TimeSpan GetDelayUntilMidnight()
//        {
//            var cairoZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
//            var nowCairo = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cairoZone);
//            var tomorrowCairo = nowCairo.Date.AddDays(1);
//            return tomorrowCairo - nowCairo;

//            //var now = DateTime.Now;
//            //var tomorrow = now.Date.AddDays(1);
//            //return tomorrow - now;

//            //return TimeSpan.FromMinutes(5);
//        }

//    }
//}


using kidsApp.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace kidsApp.Infrastructure.BackgroundServices
{
    public class DailyResetService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DailyResetService> _logger;

        public DailyResetService(IServiceScopeFactory scopeFactory, ILogger<DailyResetService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DailyResetService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var delay = GetDelayUntilMidnight();
                    _logger.LogInformation("Next daily reset in: {Delay:hh\\:mm\\:ss}", delay);

                    await Task.Delay(delay, stoppingToken);

                    if (stoppingToken.IsCancellationRequested) break;

                    await RunResetAsync();

                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in DailyResetService loop. Retrying in 1 minute.");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }

            _logger.LogInformation("DailyResetService stopped.");
        }


        private async Task RunResetAsync()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var taskLogService = scope.ServiceProvider.GetRequiredService<ITaskLogService>();

                _logger.LogInformation("Running daily reset at {Time} UTC", DateTime.UtcNow);

                await taskLogService.ResetDailyLogsAsync();

                _logger.LogInformation("Daily reset completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during daily task-log reset.");
            }
        }

        private static TimeSpan GetDelayUntilMidnight()
        {
            var cairoZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var nowCairo = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cairoZone);
            var tomorrowCairo = nowCairo.Date.AddDays(1);
            return tomorrowCairo - nowCairo;

        }
    }
}
