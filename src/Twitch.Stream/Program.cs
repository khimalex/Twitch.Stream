using static Twitch.Stream.Startup;
using Twitch.Stream.CommandsArgs;

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

       services.AddMediatRAndHandlers();

       services.Configure<Appsettings>(context.Configuration);
       services.ConfigureTwitchLibs(context.Configuration);

       services.AddHttpClient<IApiTwitchTv, HelixApiTwitchTv>();
       services.AddRefitClient<IUsherTwitchTv>()
       .ConfigureHttpClient((sp, c) =>
       {
           UsherSettings usherSettings = sp.GetRequiredService<IOptions<UsherSettings>>().Value;
           c.BaseAddress = new Uri(@"http://usher.twitch.tv");
           c.DefaultRequestHeaders.TryAddWithoutValidation("Client-ID", usherSettings.ClientIDWeb);
       });

   })
   .ConfigureLogging(ConfigureLogging);

CommandLineApplication<ShortcutsBuilder> app = null;
await builder.RunCommandLineApplicationAsync<ShortcutsBuilder>(args, c => app = c);
app?.ShowHelp();

sw.Stop();
Console.WriteLine(sw.Elapsed);
