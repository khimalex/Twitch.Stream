using System;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Twitch.Stream.App;

namespace Twitch.Stream.CommandBuilders
{
   static class DefaultBuilder
   {
      public static CommandLineApplication Build(this CommandLineApplication app, String commandName, Action<CommandLineApplication> configure, Action<HostBuilderContext, IServiceCollection> action)
      {
         app.Command(commandName, command =>
         {
            configure(command);

            var builder = new HostBuilder()
            .ConfigureHostConfiguration(b =>
            {
               b.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
               services.Configure<Appsettings>(context.Configuration);

               action(context, services);

               //services.AddHttpClient<HelixApiTwitchTv>();
               //services.AddHttpClient<UsherTwitchTv>();

               //services.AddAutoMapper(c =>
               //{
               //   c.AddProfile<HelixTwitchAuthToTwitchAuthDtoProfile>();
               //});
               //services.AddScoped<IApp, DownloadStreams>();
            })
            .ConfigureLogging(loggerBuilder =>
            {
               loggerBuilder.ClearProviders()
               .AddFilter("System", LogLevel.None)
               .AddFilter("Microsoft", LogLevel.None)
               .AddFilter("Twitch.Stream", LogLevel.Information)
               .AddSimpleConsole(c =>
               {
                  c.IncludeScopes = true;
               });
            })
            .UseConsoleLifetime();

            command.OnExecuteAsync(async ct =>
            {
               var host = builder.Build();
               using (var scope = host.Services.CreateScope())
               {
                  try
                  {
                     await scope.ServiceProvider.GetRequiredService<IApp>().RunAsync(ct);
                  }
                  catch (Exception e)
                  {

                     scope.ServiceProvider.GetRequiredService<ILogger<Program>>().LogError(e, "Произошла ошибка работы приложения!");

                  }
                  finally
                  {
                     if (scope.ServiceProvider is IDisposable disposable)
                     {
                        disposable.Dispose();
                     }
                  }
               }
               return 1;
            });



         });

         return app;

      }

   }
}
