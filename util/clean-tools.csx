using System.Linq;
var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
var toolsDir = currentDir.EnumerateDirectories().ToList().Find(x => x.Name == "tools");
if (toolsDir == null)
{
    WriteLine("Could not find tools dir, are you in a relative path to this script?");
}
if (toolsDir.GetDirectories().Length > 0)
{
    WriteLine($"Will delete all contents excluding packages.config in {toolsDir}");
    Delete();
}
else
{
    WriteLine("Nothing to delete.");
}

void Delete()
{
    toolsDir.GetFiles().ToList().ForEach(x =>
    {
        if (!x.Name.Equals("packages.config"))
        {
            x.Delete();
        }
    });

    toolsDir.GetDirectories().ToList().ForEach(x =>
    {
        Directory.Delete(x.FullName, true);
    });
}