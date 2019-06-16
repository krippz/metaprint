//////////////////////////////////////////////////////////////////////
// MODULES TOOLS AND ADINS
//////////////////////////////////////////////////////////////////////

#module nuget:?package=Cake.DotNetTool.Module&version=0.3.0

#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool dotnet:?package=GitVersion.Tool&version=5.0.0-beta3-4

#addin nuget:?package=Cake.AppVeyor&version=3.0.0
#addin nuget:?package=Refit&version=3.0.0
#addin nuget:?package=Newtonsoft.Json&version=9.0.1

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
var testResultDir = artifactsDir.Combine(Directory("test-results"));

//Project variables
var sourceDir = MakeAbsolute(Directory("src"));
var solutionPath = sourceDir.Combine("metaprint.sln").ToString();

var assemblyVersion = "1.0.0";
var packageVersion = "1.0.0";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDir);
    });

Task("restore")
    .IsDependentOn("clean")
    .Does(() =>
    {
        DotNetCoreRestore(solutionPath);
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

        Information($"AssemblySemVer: {assemblyVersion}");
        Information($"NuGetVersion: {packageVersion}");
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
            Framework = "netcoreapp2.2",
            Configuration = configuration,
            NoIncremental = true,
            NoRestore = true,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .SetVersion(assemblyVersion)
                .WithProperty("FileVersion", packageVersion)
                .WithProperty("InformationalVersion", packageVersion)
                .WithProperty("nowarn", "7035")
        };

        DotNetCoreBuild(solutionPath,settings);
    });

Task("run-tests")
    .IsDependentOn("build")
    .Does(() =>
    {
        DotNetCoreTest(solutionPath);
    });

Task("pack")
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
                .WithProperty("Copyright", $"Copyright Kristofer Linnestjerna {DateTime.Now.Year}")
        };

        pack(solutionPath, settings);
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("default")
    .IsDependentOn("pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);