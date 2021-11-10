using static Twitch.Stream.Startup;

var sw = new Stopwatch();
sw.Start();
IHostBuilder builder = Host.CreateDefaultBuilder().UseConsoleLifetime()
   .ConfigureAppConfiguration((c, b) =>
   {
#if DEBUG
       //b.AddJsonFile($@"appsettings.{c.HostingEnvironment.EnvironmentName}.json", false, true);
#endif
   })
   .ConfigureServices((context, services) =>
   {
       services.Configure<Appsettings>(context.Configuration);
       services.ConfigureTwitchLibs(context.Configuration);

       //services.AddHttpClient<KrakenApiTwitchTv>();
       services.AddHttpClient<IApiTwitchTv, HelixApiTwitchTv>();
       services.AddRefitClient<IUsherTwitchTv>()
       .ConfigureHttpClient((sp, c) =>
       {
           UsherSettings usherSettings = sp.GetRequiredService<IOptions<UsherSettings>>().Value;
           c.BaseAddress = new Uri(@"http://usher.twitch.tv");
           c.DefaultRequestHeaders.TryAddWithoutValidation("Client-ID", usherSettings.ClientIDWeb);
       });
       //services.AddHttpClient<UsherTwitchTv>();

       //todo: какую-нибудь бы фабричку
       services.AddScoped<IApp, DownloadStreams>();
       services.AddScoped<IApp, DownloadInfo>();
       services.AddScoped<IApp, DownloadVod>();
       services.AddScoped<IApp, DownloadLast>();
   })
   .ConfigureLogging(ConfigureLogging);

CommandLineApplication<ShortcutsBuilder> app = null;
await builder.RunCommandLineApplicationAsync<ShortcutsBuilder>(args, c => app = c);
app?.ShowHelp();

sw.Stop();
Console.WriteLine(sw.Elapsed);
