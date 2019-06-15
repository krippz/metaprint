using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using CSharpx;
using Extensions;

public class FileReader : IReader
{
    private readonly IEnumerable<string> fileExtensions;
    public FileReader(IEnumerable<string> fileExtensions)
    {
        var safeExt = fileExtensions.ToList();
        if (safeExt.IsNullOrEmpty())
        {
            this.fileExtensions = new List<string>
            {
                ".dll",
                ".exe"
            };
        }
        else
        {
            this.fileExtensions = safeExt;
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
        catch (FileNotFoundException)
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
        catch (Exception)
        {
            return null;
        }
    }
    private IEnumerable<string> GetFiles(IEnumerable<string> dirs)
    {
        return dirs.SelectMany(dir => Directory.GetFiles(dir, "*.*")
                                               .Where(s => fileExtensions.Contains(Path.GetExtension(s))))
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