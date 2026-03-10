using Microsoft.Extensions.Logging;

public interface IWorker
{
    public void Run(DateTime now);
}

public class Worker(ILogger<Worker> logger) : IWorker
{
    public void Run(DateTime now)
    {
        logger.LogInformation("Starting...");
    }
}