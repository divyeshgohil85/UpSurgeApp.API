using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Cron
{
    public class ForecastCronJob : CronJobService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ForecastCronJob(IScheduleConfig<ForecastCronJob> config, IServiceScopeFactory scopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _scopeFactory = scopeFactory;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var forecastService = scope.ServiceProvider.GetRequiredService<IForecastService>();
            await forecastService.Calculate();
        }
    }
}
