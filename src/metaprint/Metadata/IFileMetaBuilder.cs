namespace Metadata
{
    public interface IFileMetaInfoBuilder
    {
        IFileMetaInfoBuilder BuildVersion();
        IFileMetaInfoBuilder BuildCopyright();
        IFileMetaInfoBuilder BuildCompanyName();

        IFileMetaInfoBuilder BuildProcessorArchitecture();
        FileMetadataInfo GetMetadata();
    }
}