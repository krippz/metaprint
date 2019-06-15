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
    public FileReader(Settings settings)
    {
        fileExtensions = settings.Extensions;
    }
    private bool PathIsFile(string path)
    {
        try
        {
            return !File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
        catch
        {
            return false;
        }

    }
    private FileVersionInfo ReadFileVersionInfo(string path)
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
    private AssemblyName ReadAssemblyName(string path)
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


        var meta = new FileMetadata();
        if (i != null)
        {
            meta.FileName = Path.GetFileName(i.FileName.GetValueOrNotAvalible());
            meta.InternalName = i.InternalName.GetValueOrNotAvalible();
            meta.FileVersion = i.FileVersion.GetValueOrNotAvalible();
            meta.CompanyName = i.CompanyName.GetValueOrNotAvalible();
            meta.Copyright = i.LegalCopyright.GetValueOrNotAvalible();
            meta.Trademark = i.LegalTrademarks.GetValueOrNotAvalible();
            meta.ProductName = i.ProductName.GetValueOrNotAvalible();
            meta.ProductVersion = i.ProductVersion.GetValueOrNotAvalible();
        }
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
            var result = anyFiles.ToList().Select(x =>
            {

                return Read(x);

            });
            return result.Where(r => !r.Equals(FileMetadata.Empty));
        }

        return Enumerable.Empty<FileMetadata>();
    }
}