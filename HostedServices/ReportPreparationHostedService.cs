using rtreport.Database;
using rtreport.Services;

namespace rtreport.HostedServices
{
    public class ReportPreparationHostedService : IHostedService, IDisposable
    {
        private int TIMER_RUN_SECONDS = 10;
        private Timer _timer = null;
        private static readonly object lockObject = new object();

        private readonly ReportService reportService;
        private readonly ReportSupplierService reportSupplierService;
        public ReportPreparationHostedService(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            reportService = scope.ServiceProvider.GetService<ReportService>()!;
            reportSupplierService = scope.ServiceProvider.GetService<ReportSupplierService>()!;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork!, null, TimeSpan.Zero, TimeSpan.FromSeconds(TIMER_RUN_SECONDS));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (!Monitor.TryEnter(lockObject))
                return;

            try
            {
                var reports = reportService.GetList(5);
                foreach (var report in reports)
                {
                    var location = report.ReportRequest.Location;
                    var reportResultTask = reportSupplierService.GetReport(location);
                    reportResultTask.Wait();
                    var reportResult = reportResultTask.Result;
                    if (reportResult == null)
                        continue;

                    report.SetResult(new ReportResult(location, reportResult.PersonCount, reportResult.PhoneNumberCount));
                    reportService.UpdateRequest(report);
                }
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
