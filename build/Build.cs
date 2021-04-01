using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using System;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
   /// https://habr.com/ru/post/536208/ Быстрый старт
   /// https://habr.com/ru/post/537460/ Подробный мануал
   /// Support plugins are available for:
   ///   - JetBrains ReSharper        https://nuke.build/resharper
   ///   - JetBrains Rider            https://nuke.build/rider
   ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
   ///   - Microsoft VSCode           https://nuke.build/vscode

   public static int Main() => Execute<Build>(x => x.Compile);

   [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
   readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

   [Solution] readonly Solution Solution;

   AbsolutePath SourceDirectory => RootDirectory / "src";
   AbsolutePath TestsDirectory => RootDirectory / "tests";
   AbsolutePath OutputDirectory => RootDirectory / "output";

   Target Clean => _ => _
       .Before(Restore)
       .Executes(() =>
       {
          SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
          TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
          EnsureCleanDirectory(OutputDirectory);
       });

   Target Restore => _ => _
       .Executes(() =>
       {
          DotNetRestore(s => s
               .SetProjectFile(Solution));
       });

   Target Compile => _ => _
       .DependsOn(Restore)
       .Executes(() =>
       {
          DotNetBuild(s => s
               .SetProjectFile(Solution)
               .SetConfiguration(Configuration)
               .EnableNoRestore());
       });
   Target Publish => _ => _
   .After(Restore)
   .Executes(() =>
   {
      string[] rids = new[] { "win-x64" }; // Перечисляем RID'ы, для которых собираем приложение
      Logger.Info($"Сборка для систем: {String.Join(", ", rids)}");
      DotNetPublish(s => s // Теперь вызываем dotnet publish
           .SetVerbosity(DotNetVerbosity.Detailed)
           .SetVersion("2.0.0.0")
           .SetProject(Solution.GetProject("Twitch.Stream")) // Для dotnet publish желательно указывать проект
           .SetPublishSingleFile(true) // Собираем в один файл
           .SetSelfContained(true)     // Вместе с рантаймом
           .SetPublishTrimmed(true)
           .SetConfiguration(Configuration) // Для определённой конфигурации
           .AddProperty("IncludeNativeLibrariesForSelfExtract", true) //добавляем параметр в публикацию, чтобы нативные библиотеки были включены в финальный exe
           .CombineWith(rids, (s, rid) => s // Но нам нужны разные комбинации параметров
               .SetRuntime(rid) // Устанавливаем RID
               .SetOutput(OutputDirectory / rid))); // Делаем так, чтобы сборки с разными RID попали в разные директории
   });

}
