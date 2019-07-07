using System.Text;
using metaprint.Print;

namespace metaprint.Metadata
{
    public class FileMetadataInfo
    {
        [WrapPretty]
        public string Version { private get; set; }

        [WrapPretty]
        public string Copyright { private get; set; }

        [WrapPretty]
        public string CompanyName { private get; set; }

        [WrapPretty]
        public string ProcessorArchitecture { private get; set; }

        [WrapPretty]
        public string AuthentiCodeCertificateThumbprint { private get; set; }

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