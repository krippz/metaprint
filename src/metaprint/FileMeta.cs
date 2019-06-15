using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class FileMetadata
{
    public string CompanyName { get; set; }
    public string FileVersion { get; set; }
    public string FileName { get; set; }
    public string Copyright { get; set; }
    public string Trademark { get; set; }
    public string ProductName { get; set; }
    public string ProductVersion { get; set; }
    public string ProcessorArchitecture { get; set; }
}
public class FileMetadataInfo
{
    public string Version { get; set; }
    public string Copyright { get; set; }
    public string CompanyName { get; set; }

    public string ProcessorArchitecture { get; set; }

    public override string ToString() =>
            new StringBuilder()
            .AppendLine(Version)
            .AppendLine(CompanyName)
            .AppendLine(Copyright)
            .AppendLine(ProcessorArchitecture)
            .ToString();
}

public interface IFileMetaInfoBuilder
{
    IFileMetaInfoBuilder BuildVersion();
    IFileMetaInfoBuilder BuildCopyright();
    IFileMetaInfoBuilder BuildCompanyName();

    IFileMetaInfoBuilder BuildProcessorArchitecture();
    FileMetadataInfo GetMetadata();
}

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
        _fileMetadataInfo.Copyright = string.Join(Environment.NewLine, items.Select(item => $"{item.FileName,-75}{item.Copyright}"));
        return this;
    }
    public IFileMetaInfoBuilder BuildCompanyName()
    {
        var items = _reader.Read(_filesA);
        _fileMetadataInfo.CompanyName = string.Join(Environment.NewLine, items.Select(item => $"{item.FileName,-75}{item.CompanyName}"));
        return this;
    }

    public IFileMetaInfoBuilder BuildProcessorArchitecture()
    {
        var items = _reader.Read(_filesA);
        _fileMetadataInfo.ProcessorArchitecture = string.Join(Environment.NewLine, items.Select(item => $"{item.FileName,-75}{item.ProcessorArchitecture}"));
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

public class FileMetadataInfoDirector
{
    private readonly IFileMetaInfoBuilder _fileMetadataInfoBuilder;
    public FileMetadataInfoDirector(IFileMetaInfoBuilder fileMetadataInfoBuilder)
    {
        _fileMetadataInfoBuilder = fileMetadataInfoBuilder;
    }

    public void BuildFileMetadataInfo(bool verbose = false)
    {
        _fileMetadataInfoBuilder.BuildVersion();

        if (verbose)
        {
            _fileMetadataInfoBuilder.BuildCopyright();
            _fileMetadataInfoBuilder.BuildCompanyName();
            _fileMetadataInfoBuilder.BuildProcessorArchitecture();
        }
    }
}