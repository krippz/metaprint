namespace Metadata
{
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
}