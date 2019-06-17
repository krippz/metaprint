using System.Text;

namespace Metadata
{
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
}