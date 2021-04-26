using System;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
internal class Build : NukeBuild
{
   /// https://habr.com/ru/post/536208/ ������� �����
   /// https://habr.com/ru/post/537460/ ��������� ������
   /// Support plugins are available for:
   ///   - JetBrains ReSharper        https://nuke.build/resharper
   ///   - JetBrains Rider            https://nuke.build/rider
   ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
   ///   - Microsoft VSCode           https://nuke.build/vscode

   public static System.Int32 Main() => Execute<Build>(x => x.Compile);

   [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
#pragma warning disable IDE1006 // Naming Styles
   private readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

   [Solution] private readonly Solution Solution;
#pragma warning restore IDE1006 // Naming Styles

   private AbsolutePath SourceDirectory => RootDirectory / "src";

   private AbsolutePath TestsDirectory => RootDirectory / "tests";

   private AbsolutePath OutputDirectory => RootDirectory / "output";

   private Target Clean => _ => _
       .Before(Restore)
       .Executes(() =>
       {
          SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
          TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
          EnsureCleanDirectory(OutputDirectory);
       });

   private Target Restore => _ => _
       .Executes(() =>
       {
          DotNetRestore(s => s
               .SetProjectFile(Solution));
       });

   private Target Compile => _ => _
       .DependsOn(Restore)
       .Executes(() =>
       {
          DotNetBuild(s => s
               .SetProjectFile(Solution)
               .SetConfiguration(Configuration)
               .EnableNoRestore());
       });

   private Target Publish => _ => _
   .After(Restore)
   .Executes(() =>
   {
      System.String[] rids = new[] { "win-x64" }; // ����������� RID'�, ��� ������� �������� ����������
      Logger.Info($"������ ��� ������: {String.Join(", ", rids)}");
      DotNetPublish(s => s // ������ �������� dotnet publish
           .SetVerbosity(DotNetVerbosity.Normal)
           .SetVersion("2.5.0.0")
           .SetProject(Solution.GetProject("Twitch.Stream")) // ��� dotnet publish ���������� ��������� ������
           .SetPublishSingleFile(true) // �������� � ���� ����
           .SetSelfContained(true)     // ������ � ���������
           .SetPublishTrimmed(true)
           .SetConfiguration(Configuration) // ��� ����������� ������������
           .AddProperty("IncludeNativeLibrariesForSelfExtract", true) //��������� �������� � ����������, ����� �������� ���������� ���� �������� � ��������� exe
           .CombineWith(rids, (s, rid) => s // �� ��� ����� ������ ���������� ����������
               .SetRuntime(rid) // ������������� RID
               .SetOutput(OutputDirectory / rid))); // ������ ���, ����� ������ � ������� RID ������ � ������ ����������
   });

}
