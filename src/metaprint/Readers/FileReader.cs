using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using metaprint.Config;
using metaprint.Extensions;
using metaprint.Metadata;
using PeNet;

namespace metaprint.Readers
{
    public class FileReader : IReader
    {
        private readonly IEnumerable<string> _fileExtensions;

        public FileReader(Settings settings)
        {
            _fileExtensions = settings.Extensions;
        }
        public static FileMetadata Read(string path)
        {
            var meta = new FileMetadata();

            ReadFileAs(path, ReadFileVersionInfo, meta);
            ReadFileAs(path, ReadAssemblyName, meta);
            ReadFileAs(path, ReadPeFile, meta);

            return meta;
        }

        public IEnumerable<FileMetadata> Read(IEnumerable<string> paths)
        {
            var anyFiles = GetFiles(paths);

            // ReSharper disable PossibleMultipleEnumeration
            if (anyFiles.IsNullOrEmpty())
            {
                return Enumerable.Empty<FileMetadata>();
            }

            var result = anyFiles.ToList().Select(Read);
            // ReSharper restore PossibleMultipleEnumeration

            return result.Where(r => !r.Equals(FileMetadata.Empty));

        }
        private IEnumerable<string> GetFiles(IEnumerable<string> dirs)
        {
            return dirs.SelectMany(dir => Directory.GetFiles(dir, "*.*")
                                                .Where(s => _fileExtensions.Contains(Path.GetExtension(s))))
                                                .ToList();
        }
        private static bool PathIsFile(string path)
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
        private static T Execute<T>(Func<T> function)
        {
            try
            {
                return function();
            }
            catch
            {
                return default;
            }
        }
        private static FileVersionInfo ReadFileVersionInfo(string path)
        {
            return PathIsFile(path) ? Execute(() => FileVersionInfo.GetVersionInfo(path)) : null;
        }
        private static AssemblyName ReadAssemblyName(string path)
        {
            return PathIsFile(path) ? Execute(() => AssemblyName.GetAssemblyName(path)) : null;
        }
        private static PeFile ReadPeFile(string path)
        {
            return PathIsFile(path) ? Execute(() => new PeFile(path)) : null;
        }
        private static void ReadFileAs<T>(string path, Func<string, T> reader, FileMetadata data)
        {
            var result = reader(path);
            if (result == null)
            {
                return;
            }

            switch (result)
            {
                case FileVersionInfo info:
                    data.FileName = Path.GetFileName(info.FileName.GetValueOrNotAvailable());
                    data.InternalName = info.InternalName.GetValueOrNotAvailable();
                    data.FileVersion = info.FileVersion.GetValueOrNotAvailable();
                    data.CompanyName = info.CompanyName.GetValueOrNotAvailable();
                    data.Copyright = info.LegalCopyright.GetValueOrNotAvailable();
                    data.Trademark = info.LegalTrademarks.GetValueOrNotAvailable();
                    data.ProductName = info.ProductName.GetValueOrNotAvailable();
                    data.ProductVersion = info.ProductVersion.GetValueOrNotAvailable();
                    break;

                case AssemblyName asm:
                    data.ProcessorArchitecture = Enum.GetName(typeof(ProcessorArchitecture), asm.ProcessorArchitecture);
                    break;

                case PeFile pe:
                    data.Bitness = pe.Is64Bit ? "x64" : "x32";
                    data.AuthentiCodeCertificateThumbprint = pe.IsSigned ? pe.Authenticode?.SigningCertificate.Thumbprint.GetValueOrNotAvailable() : "not signed";
                    break;
            }
        }
    }
}