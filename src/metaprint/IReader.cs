using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using CSharpx;
using versions;


public interface IReader
{
    IEnumerable<FileMetadata> Read(IEnumerable<string> paths);
    FileMetadata Read(string path);
}




public class FileReader : IReader
{
    private readonly IEnumerable<string> extensions;
    public FileReader(IEnumerable<string> extensions)
    {
        var safeExt = extensions.ToList();
        if (safeExt.IsNullOrEmpty())
        {
            this.extensions = new List<string>
            {
                ".dll",
                ".exe"
            };
        }
        else
        {
            this.extensions = safeExt;
        }

    }
    private bool PathIsFile(string path)
    {
        return !File.GetAttributes(path).HasFlag(FileAttributes.Directory);
    }
    public FileVersionInfo ReadFileVersionInfo(string path)
    {
        if (!PathIsFile(path))
        {
            return null;
        }

        try
        {
            return System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
        }
        catch (FileNotFoundException e)
        {
            return null;
        }

    }
    public AssemblyName ReadAssemblyName(string path)
    {
        if (!PathIsFile(path))
        {
            return null;
        }
        try
        {
            return System.Reflection.AssemblyName.GetAssemblyName(path);
        }
        catch (Exception e)
        {
            return null;
        }
    }
    private IEnumerable<string> GetFiles(IEnumerable<string> dirs)
    {
        return dirs.SelectMany(dir => Directory.GetFiles(dir, "*.*")
                                               .Where(s => extensions.Contains(Path.GetExtension(s))))
                                               .ToList();
    }
    public FileMetadata Read(string path)
    {
        FileVersionInfo i = ReadFileVersionInfo(path);
        AssemblyName x = ReadAssemblyName(path);

        if (i == null)
        {
            return new FileMetadata { FileName = "N/A" };
        }

        var meta = new FileMetadata
        {
            FileName = i.InternalName,
            FileVersion = i.FileVersion,
            CompanyName = i.CompanyName,
            Copyright = i.LegalCopyright,
            Trademark = i.LegalTrademarks,
            ProductName = i.ProductName,
            ProductVersion = i.ProductVersion
        };

        if (x != null)
        {
            meta.ProcessorArchitecture = Enum.GetName(typeof(ProcessorArchitecture), x.ProcessorArchitecture);
        }

        return meta;
    }
    public IEnumerable<FileMetadata> Read(IEnumerable<string> paths)
    {
        var anyFiles = GetFiles(paths);

        if (!anyFiles.IsNullOrEmpty())
        {
            return anyFiles.ToList().Select(x =>
            {
                return Read(x);

            });
        }

        return Enumerable.Empty<FileMetadata>();
    }
}

public static class Extensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
    {
        return items == null || !items.Any();
    }
}
