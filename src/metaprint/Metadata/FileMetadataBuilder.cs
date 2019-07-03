using System;
using System.Collections.Generic;
using System.Linq;
using Readers;
using Extensions;

namespace Metadata
{
    public class FileMetadataBuilder : IFileMetaInfoBuilder
    {
        private FileMetadataInfo _fileMetadataInfo;
        private IEnumerable<FileMetadata> _files;
        private IEnumerable<string> _filesA;
        private IReader _reader;

        public FileMetadataBuilder(IEnumerable<FileMetadata> files)
        {
            _files = files;
            _fileMetadataInfo = new FileMetadataInfo();
        }
        public FileMetadataBuilder(IEnumerable<string> files, IReader reader)
        {
            _filesA = files;
            _reader = reader;
            _fileMetadataInfo = new FileMetadataInfo();
        }

        public IFileMetaInfoBuilder BuildVersion()
        {
            var items = _reader.Read(_filesA);
            _fileMetadataInfo.Version = string.Join(Environment.NewLine, items.Select(item => $"{item.FileName,-75}{item.FileVersion.Truncate(50),50}"));
            return this;
        }

        public IFileMetaInfoBuilder BuildCopyright()
        {
            var items = _reader.Read(_filesA);
            _fileMetadataInfo.Copyright = string.Join(Environment.NewLine, items.Select(item => $"{item.FileName,-75}{item.Copyright,50}"));
            return this;
        }
        public IFileMetaInfoBuilder BuildCompanyName()
        {
            var items = _reader.Read(_filesA);
            _fileMetadataInfo.CompanyName = string.Join(Environment.NewLine, items.Select(item => $"{item.FileName,-75}{item.CompanyName,50}"));
            return this;
        }

        public IFileMetaInfoBuilder BuildProcessorArchitecture()
        {
            var items = _reader.Read(_filesA);
            _fileMetadataInfo.ProcessorArchitecture = string.Join(Environment.NewLine, items.Select(item => $"{item.FileName,-75}{item.ProcessorArchitecture,50}"));
            return this;
        }

        public IFileMetaInfoBuilder BuildAuthentiCodeCertificateThumbprint()
        {
            var items = _reader.Read(_filesA);
            _fileMetadataInfo.AuthentiCodeCertificateThumbprint = string.Join(Environment.NewLine, items.Select(item => $"{item.FileName,-75}{item.AuthentiCodeCertificateThumbprint,50}"));

            return this;
        }

        public FileMetadataInfo GetMetadata()
        {
            var meta = _fileMetadataInfo;

            Clear();
            return meta;
        }

        private void Clear() => _fileMetadataInfo = new FileMetadataInfo();
    }
}