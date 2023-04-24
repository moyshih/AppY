using AppY.Services;

Console.WriteLine("Starting AppY..");

// Configure workers
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, configBuilder) =>
    {
        configBuilder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // create an instance of ConfigData to bind the configuration values to
        var configData = new ConfigData();
        context.Configuration.GetSection("Config").Bind(configData);
        services.AddSingleton(configData);

        services.AddSingleton<IMessagesService, RabbitMQService>();
        services.AddHostedService<CreateMotorcyclesWorker>();
        services.AddHostedService<SubscribeForVehiclesWorker>();
    })
    .Build();
var messageService = host.Services.GetService<IMessagesService>();

// Continue only if there is a connection to the messsges server
if (messageService.IsConnected)
{
    // Run the workers
    host.RunAsync();

    Console.WriteLine("Press any key to exit..");
    Console.ReadLine();

    host.StopAsync();
}