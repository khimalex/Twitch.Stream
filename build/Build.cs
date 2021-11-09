﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
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

    public static int Main()
    {
        return Execute<Build>(x => x.Compile);
    }

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitVersion] private readonly GitVersion GitVersion;
    [Solution] private readonly Solution Solution;
    [Parameter] private readonly bool Trim;

    private AbsolutePath SourceDirectory => RootDirectory / "src";

    private AbsolutePath TestsDirectoryResult => RootDirectory / "tests";

    private AbsolutePath OutputDirectory => RootDirectory / "output";

    private Target Clean => _ => _
        .Executes(() =>
        {
            Solution.AllProjects
            .Where(p => !p.Name.Contains("_", StringComparison.InvariantCultureIgnoreCase))
            .SelectMany(p => p.Directory.GlobDirectories("**/bin", "**/obj"))
            .ForEach(DeleteDirectory);
            DeleteDirectory(OutputDirectory);
            //another way of cleaning directories
            //EnsureCleanDirectory(OutputDirectory);
        });
    private Target CleanTests => _ => _
        .Executes(() =>
        {
            Solution.AllProjects
            .Where(p => p.Name.Contains("test", StringComparison.InvariantCultureIgnoreCase))
            .SelectMany(p => p.Directory.GlobDirectories("**/bin", "**/obj"))
            .ForEach(DeleteDirectory);
            DeleteDirectory(TestsDirectoryResult);
            //another way of cleaning directories
            //EnsureCleanDirectory(OutputDirectory);
        });

    private Target Restore => _ => _
        //.After(Clean)
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                  .SetProjectFile(Solution)
                  .SetVerbosity(DotNetVerbosity.Normal)
                  .When(InvokedTargets.Contains(Publish), ss => ss.SetVerbosity(DotNetVerbosity.Quiet)));

        });

    private Target Compile => _ => _
        //.After(Restore)
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetVerbosity(DotNetVerbosity.Normal)
                //.EnableNoRestore()
                .When(InvokedTargets.Contains(Publish), ss => ss.SetVerbosity(DotNetVerbosity.Minimal)));
        });

    private Target Publish => _ => _
    .DependsOn(Compile)
    .Executes(() =>
    {
        string[] rids = new[] { "win-x64", "win-x86" };
        IEnumerable<Project> publishProjects = Solution.AllProjects
        .Where(p => !p.Name.Contains("test", StringComparison.InvariantCultureIgnoreCase))
        .Where(p => !p.Name.Contains("_", StringComparison.InvariantCultureIgnoreCase));
        DotNetPublish(s => s
            .SetConfiguration(Configuration)
            .SetVerbosity(DotNetVerbosity.Quiet)
            .SetVersion(GitVersion.AssemblySemVer)
            .SetAssemblyVersion(GitVersion.AssemblySemVer)
            .SetFileVersion(GitVersion.AssemblySemFileVer)
            .SetInformationalVersion(GitVersion.InformationalVersion)
            .SetAuthors("KhimAlex")
            .AddProperty("IncludeNativeLibrariesForSelfExtract", true)
            .CombineWith(publishProjects, (s, project) => s
                .SetProject(project)
                .When("exe".Equals(project.GetOutputType(), StringComparison.InvariantCultureIgnoreCase), s => s
                    .SetPublishSingleFile(true)
                    .SetSelfContained(true)
                    .SetPublishTrimmed(Trim)
                    .When(project.GetTargetFrameworks().Any(f => f.Contains("windows", StringComparison.InvariantCultureIgnoreCase)), s => s
                        .SetPublishTrimmed(false)
                        .SetPublishSingleFile(false)
                    )
                )
                .CombineWith(rids, (s, rid) => s
                    .SetRuntime(rid)
                    .SetSelfContained(true)
                    .SetPublishTrimmed(true)
                    .SetOutput(OutputDirectory / project.Name / Configuration / rid)
                )
            )
        );
    });

}
