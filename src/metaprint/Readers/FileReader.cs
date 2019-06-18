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
        public FileMetadata Read(string path)
        {
            var meta = new FileMetadata();

            ReadFileAs<FileVersionInfo>(path, ReadFileVersionInfo, meta);
            ReadFileAs<AssemblyName>(path, ReadAssemblyName, meta);
            ReadFileAs<PeFile>(path, ReadPeFile, meta);

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
        private IEnumerable<string> GetFiles(IEnumerable<string> dirs)
        {
            return dirs.SelectMany(dir => Directory.GetFiles(dir, "*.*")
                                                .Where(s => fileExtensions.Contains(Path.GetExtension(s))))
                                                .ToList();
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
        private T Execute<T>(Func<T> function)
        {
            try
            {
                return function();
            }
            catch
            {
                return default(T);
            }
        }
        private FileVersionInfo ReadFileVersionInfo(string path)
        {
            return PathIsFile(path) ? Execute(() => System.Diagnostics.FileVersionInfo.GetVersionInfo(path)) : null;
        }
        private AssemblyName ReadAssemblyName(string path)
        {
            return PathIsFile(path) ? Execute(() => System.Reflection.AssemblyName.GetAssemblyName(path)) : null;
        }
        private PeFile ReadPeFile(string path)
        {
            return PathIsFile(path) ? Execute(() => new PeFile(path)) : null;
        }
        private void ReadFileAs<T>(string path, Func<string, T> reader, FileMetadata data)
        {
            T result = reader(path);
            if (result == null)
            {
                return;
            }

            switch (result)
            {
                case FileVersionInfo info:
                    data.FileName = Path.GetFileName(info.FileName.GetValueOrNotAvalible());
                    data.InternalName = info.InternalName.GetValueOrNotAvalible();
                    data.FileVersion = info.FileVersion.GetValueOrNotAvalible();
                    data.CompanyName = info.CompanyName.GetValueOrNotAvalible();
                    data.Copyright = info.LegalCopyright.GetValueOrNotAvalible();
                    data.Trademark = info.LegalTrademarks.GetValueOrNotAvalible();
                    data.ProductName = info.ProductName.GetValueOrNotAvalible();
                    data.ProductVersion = info.ProductVersion.GetValueOrNotAvalible();
                    break;
                case AssemblyName asm:
                    data.ProcessorArchitecture = Enum.GetName(typeof(ProcessorArchitecture), asm.ProcessorArchitecture);
                    break;
                case PeFile pe:
                    data.Bitness = pe.Is64Bit ? "x64" : "x32";
                    break;
                default:
                    break;
            }
        }
    }
}