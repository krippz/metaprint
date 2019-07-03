using System.Text;
using Print;

namespace Metadata
{
    public class FileMetadataInfo
    {
        [WrapPretty]
        public string Version { get; set; }

        [WrapPretty]
        public string Copyright { get; set; }

        [WrapPretty]
        public string CompanyName { get; set; }

        [WrapPretty]
        public string ProcessorArchitecture { get; set; }

        [WrapPretty]
        public string AuthentiCodeCertificateThumbprint { get; set; }

        public override string ToString() =>
                new StringBuilder()
                .AppendLine(Version)
                .AppendLine(CompanyName)
                .AppendLine(Copyright)
                .AppendLine(ProcessorArchitecture)
                .AppendLine(AuthentiCodeCertificateThumbprint)
                .ToString();
    }
}