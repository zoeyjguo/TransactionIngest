const string loggerTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}] [{SourceContext:l}] {Message:lj}{NewLine}{Exception}";
var baseDir = AppDomain.CurrentDomain.BaseDirectory;
var logfile = Path.Combine(baseDir, "AppData", "logs", "log.txt");
var loggerConfig = new Serilog.LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(LogEventLevel.Information, loggerTemplate, theme: AnsiConsoleTheme.Literate)
    .WriteTo.File(logfile, LogEventLevel.Information, loggerTemplate, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 366);
Log.Logger = loggerConfig.CreateLogger();

try
{
    Log.Information("====================================================================");
    Log.Information($"Application Starts. Version: {System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version}");
    Log.Information($"Application Directory: {baseDir}");

    if (OperatingSystem.IsWindows())
    {
        var userName = WindowsIdentity.GetCurrent().Name;
        Log.Information($"The runner account is [{userName}].");
    }

    var builder = Host.CreateDefaultBuilder(args)
        .UseContentRoot(baseDir)
        .ConfigureHostConfiguration(config =>
        {
            config.AddJsonFile("config/hostsettings.json", true, true)
                .AddJsonFile($"config/hostsettings.{Environment.MachineName}.json", true, true);
            config.AddCommandLine(args);
        })
        .ConfigureAppConfiguration((context, config) =>
        {
            Log.Information($"Hosting Environment: {context.HostingEnvironment.EnvironmentName}; Hosting Machine: {Environment.MachineName}\r\n");
            config.AddJsonFile("config/appsettings.json", false, true)
                .AddJsonFile($"config/appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddJsonFile($"config/appsettings.{Environment.MachineName}.json", true, true);
        })
        .ConfigureServices((hostContext, services) =>
        {
            //services.AddSingleton(hostContext.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()!);
            //services.AddSingleton<IWorker, Worker>();
            //services.AddSingleton<ISplitPdfProcessor, SplitPdfProcessor>();
            //services.AddSingleton<ISummaryEmailService, SummaryEmailService>();
        }).UseSerilog();

    var host = builder.Build();
    using var scope = host.Services.CreateScope();
    var worker = scope.ServiceProvider.GetRequiredService<IWorker>();
    worker.Run(DateTime.Now);
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.Information("====================================================================\r\n");
    Log.CloseAndFlush();
}

