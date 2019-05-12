using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

    public FileVersionInfo ReadFileVersionInfo(string path)
    {
        try
        {
            var attr = File.GetAttributes(path);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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

        return (i == null) ? new FileMetadata { FileName = "N/A" } : new FileMetadata
        {
            FileName = i.InternalName,
            FileVersion = i.FileVersion,
            CompanyName = i.CompanyName,
            Copyright = i.LegalCopyright
        };
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
