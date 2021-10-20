//////////////////////////////////////////////////////////////////////
// MODULES TOOLS AND ADDINS
//////////////////////////////////////////////////////////////////////
#tool dotnet:?package=GitVersion.Tool&version=5.7.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var artifactsDir = MakeAbsolute(Directory("artifacts"));
var packagesDir = artifactsDir.Combine(Directory("nupkg"));
var testResultsDir = artifactsDir.Combine(Directory("test-results"));

//Project variables
var sourceDir = MakeAbsolute(Directory("src"));
var solutionPath = "metaprint.sln";

var assemblyVersion = "1.0.0";
var packageVersion = "1.0.0";
var informationalVersion = "";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDir);
        var projectFiles = GetFiles("./**/*.csproj");

        foreach (var projectFile in projectFiles)
        {
            var binDir = projectFile.GetDirectory().Combine("bin");
            var objDir = projectFile.GetDirectory().Combine("obj");

            if(DirectoryExists(binDir))
            {
                 DeleteDirectory(binDir, new DeleteDirectorySettings{ Force = true, Recursive=true});
            }
            if(DirectoryExists(objDir))
            {
                DeleteDirectory(objDir, new DeleteDirectorySettings{ Force = true, Recursive=true});
            }
        }
    });

Task("restore")
    .IsDependentOn("clean")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

Task("semver")
    .IsDependentOn("restore")
    .Does(() =>
    {
        var settings = new GitVersionSettings
        {
            NoFetch = true
        };

        var gitVersion = GitVersion(settings);
        assemblyVersion = gitVersion.AssemblySemVer;
        packageVersion = gitVersion.NuGetVersion;
        informationalVersion = gitVersion.InformationalVersion;

        Information($"AssemblySemVer: {assemblyVersion}");
        Information($"NuGetVersion: {packageVersion}");
        Information($"InformationalVersion: {informationalVersion}");
    });

Task("set-appveyor-version")
    .IsDependentOn("semver")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
    {
        AppVeyor.UpdateBuildVersion(packageVersion);
    });

Task("build")
    .IsDependentOn("set-appveyor-version")
    .Does(() =>
    {
        var settings = new DotNetCoreBuildSettings
        {
            Configuration = configuration,
            NoIncremental = true,
            NoRestore = true,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .SetVersion(assemblyVersion)
                .SetFileVersion(packageVersion)
                .SetInformationalVersion(packageVersion)
                .WithProperty("nowarn", "7035")
                .WithProperty("Copyright", $"Kristofer Linnestjerna {DateTime.Now.Year}")
        };

        GetFiles("./src/*/*.csproj")
            .ToList()
            .ForEach(f => DotNetCoreBuild(f.FullPath, settings));

        GetFiles("./tests/*/*.test.csproj")
            .ToList()
            .ForEach(f => DotNetCoreBuild(f.FullPath, settings));

    });

Task("run-tests")
    .IsDependentOn("build")
    .Does(() =>
    {
        var argumentsBuilder = new ProcessArgumentBuilder()
                                   .Append("test")
                                   .Append("--no-build")
                                   .Append("--test-adapter-path:.");

        var settings = new ProcessSettings()
        {
            Arguments = argumentsBuilder,
        };

        var projectFiles = GetFiles("./tests/*/*.test.csproj");

        foreach (var projectFile in projectFiles)
        {
            var testResultsFile = testResultsDir.Combine($"{projectFile.GetFilenameWithoutExtension()}.xml");
            var arguments = $"{argumentsBuilder.Render()} --logger:\"xunit;LogFilePath={testResultsFile}\"";

            settings.WorkingDirectory = projectFile.GetDirectory();
            settings.Arguments = arguments;

            StartProcess("dotnet", settings);
        }
    })
    // .Does(() =>
    // {

    // })
    .DeferOnError();

Task("package-nuget")
    .IsDependentOn("run-tests")
    .WithCriteria(() => HasArgument("pack"))
    .Does(() =>
    {
        var settings = new DotNetCorePackSettings
        {
            Configuration = configuration,
            NoBuild = true,
            NoRestore = true,
            IncludeSymbols = true,
            OutputDirectory = packagesDir,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .WithProperty("PackageVersion", packageVersion)
                .WithProperty("Copyright", $"Kristofer Linnestjerna {DateTime.Now.Year}")
        };

        GetFiles("./src/*/*.csproj")
            .ToList()
            .ForEach(f => DotNetCorePack(f.FullPath, settings));
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("default")
    .IsDependentOn("package-nuget");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);