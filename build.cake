#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./artifacts/bin") + Directory(configuration);
var packDir = buildDir + Directory("nupkg");

//Project variables
var sourceDir = new DirectoryPath("./src");
var solutionName = "metaprint.sln";

var projectPath = sourceDir.CombineWithFilePath(solutionName);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("restore")
    .IsDependentOn("clean")
    .Does(() =>
{
    DotNetCoreRestore(projectPath.ToString());
});

Task("build")
    .IsDependentOn("restore")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Framework = "netcoreapp2.2",
        Configuration = configuration,
        OutputDirectory = buildDir
    };

    DotNetCoreBuild("./src/metaprint.sln",settings);

});

Task("run-tests")
    .IsDependentOn("build")
    .Does(() =>
{

});

Task("pack")
    .IsDependentOn("run-tests")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
     {
         Configuration = configuration,
         OutputDirectory = packDir
     };
    DotNetCorePack("./src/metaprint.sln", settings);
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
