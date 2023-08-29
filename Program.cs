IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("storedprocedures.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<ToDoSomethingBackgroundTask>();
        services.AddService(hostContext.Configuration);
        StoredProcedures.InitStoredProcedureSetting(hostContext.Configuration);
    })
    .Build();

await host.RunAsync();
