namespace metaprint.Metadata
{
    public interface IFileMetaInfoBuilder
    {
        IFileMetaInfoBuilder BuildVersion();
        IFileMetaInfoBuilder BuildCopyright();
        IFileMetaInfoBuilder BuildCompanyName();
        IFileMetaInfoBuilder BuildProcessorArchitecture();
        IFileMetaInfoBuilder BuildAuthentiCodeCertificateThumbprint();
    }
}