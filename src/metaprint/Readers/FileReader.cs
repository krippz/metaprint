using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using CSharpx;
using Extensions;
using PeNet;
using Readers;
using Config;
using Metadata;

namespace Readers
{
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
            catch
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
            catch
            {
                return null;
            }
        }
        private PeFile ReadPeFile(string path)
        {
            if (!PathIsFile(path))
            {
                return null;
            }
            try
            {
                return new PeFile(path);
            }
            catch
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
            FileVersionInfo info = ReadFileVersionInfo(path);
            AssemblyName assembly = ReadAssemblyName(path);
            PeFile pefile = ReadPeFile(path);

            var meta = new FileMetadata();
            if (info != null)
            {
                meta.FileName = Path.GetFileName(info.FileName.GetValueOrNotAvalible());
                meta.InternalName = info.InternalName.GetValueOrNotAvalible();
                meta.FileVersion = info.FileVersion.GetValueOrNotAvalible();
                meta.CompanyName = info.CompanyName.GetValueOrNotAvalible();
                meta.Copyright = info.LegalCopyright.GetValueOrNotAvalible();
                meta.Trademark = info.LegalTrademarks.GetValueOrNotAvalible();
                meta.ProductName = info.ProductName.GetValueOrNotAvalible();
                meta.ProductVersion = info.ProductVersion.GetValueOrNotAvalible();
            }
            if (assembly != null)
            {
                meta.ProcessorArchitecture = Enum.GetName(typeof(ProcessorArchitecture), assembly.ProcessorArchitecture);
            }
            if (pefile != null)
            {
                meta.Bitness = pefile.Is64Bit ? "x64" : "x32";
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

                return result.Where(r => !r.Equals(Metadata.FileMetadata.Empty));
            }

            return Enumerable.Empty<FileMetadata>();
        }
    }
}