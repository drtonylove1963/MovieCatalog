namespace MovieCatalog.Infrastructure.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Marten;
using Marten.Events.Daemon;
using System;
using System.Threading;
using System.Threading.Tasks;

// Background service to ensure the event daemon is running for Marten's asynchronous projections
public class EventStoreService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventStoreService> _logger;

    public EventStoreService(
        IServiceProvider serviceProvider,
        ILogger<EventStoreService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Event Store Service is starting...");

        try
        {
            // This service will use a scoped DocumentStore to start the daemon
            // This ensures that the daemon is running for the lifetime of the application
            using var scope = _serviceProvider.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<IDocumentStore>();

            // Apply database changes with a timeout
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, stoppingToken);
            
            try
            {
                await store.Storage.ApplyAllConfiguredChangesToDatabaseAsync();
                _logger.LogInformation("Database schema applied successfully");
            }
            catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
            {
                _logger.LogWarning("Database schema application timed out after 30 seconds");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying database schema changes");
                return;
            }

            // Get the daemon and start it
            try
            {
                var daemon = await store.BuildProjectionDaemonAsync();
                await daemon.StartAllAsync();
                _logger.LogInformation("Event Store Service started successfully");

                // Keep the service running until the application is stopped
                try
                {
                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Normal termination when the application is shutting down
                    _logger.LogInformation("Event Store Service is shutting down...");
                    
                    await daemon.StopAllAsync();
                    
                    _logger.LogInformation("Event Store Service stopped successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting or running the event daemon");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Event Store Service");
        }
    }
}
