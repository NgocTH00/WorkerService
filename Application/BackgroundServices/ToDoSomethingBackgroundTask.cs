namespace SingleService.Application.BackgroundServices;

public class ToDoSomethingBackgroundTask : BackgroundService
{
    private readonly ILogger<ToDoSomethingBackgroundTask> _logger;
    private readonly IDemoDbService _demoDbService;
    private readonly IKafkaProduceService _kafkaProduceService;

    public ToDoSomethingBackgroundTask(
        IDemoDbService demoDbService,
        IKafkaProduceService kafkaProduceService,
        ILogger<ToDoSomethingBackgroundTask> logger)
    {
        _logger = logger;
        _demoDbService = demoDbService;
        _kafkaProduceService = kafkaProduceService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var text = $"Worker {StoredProcedures.DemoUpdate} running at: {DateTimeOffset.Now}";
            _kafkaProduceService.Produce(text, "demo", "key");
            _logger.LogInformation(text);
            await Task.Delay(1000, stoppingToken);
        }
    }
}