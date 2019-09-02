using System.Collections.Generic;

namespace metaprint.Metadata
{
    public class FileMetadata
    {
        public string CompanyName { get; set; }
        public string FileVersion { get; set; }
        public string FileName { get; set; }
        public string InternalName { get; set; }
        public string Copyright { get; set; }
        public string Trademark { get; set; }
        public string ProductName { get; set; }
        public string ProductVersion { get; set; }
        public string ProcessorArchitecture { get; set; }
        public string Bitness { get; set; }
        public string AuthentiCodeCertificateThumbprint { get; set; }
        public static FileMetadata Empty => new FileMetadata();
        public FileMetadata()
        {
           FileName = string.Empty;
           InternalName = string.Empty;
           FileVersion = string.Empty;
           CompanyName = string.Empty;
           Copyright = string.Empty;
           Trademark = string.Empty;
           ProductName = string.Empty;
           ProductVersion = string.Empty;
           ProcessorArchitecture = string.Empty;
           Bitness = string.Empty;
           AuthentiCodeCertificateThumbprint = string.Empty;
        }
    }

    public class FileMetadataComparer : IEqualityComparer<FileMetadata>
    {
        public bool Equals(FileMetadata x, FileMetadata y)
        {
            
            return x != null && y != null && x.FileName.Equals(y.FileName) &&
                                             x.InternalName.Equals(y.InternalName) &&
                                             x.FileVersion.Equals(y.FileVersion) &&
                                             x.CompanyName.Equals(y.CompanyName) &&
                                             x.Copyright.Equals(y.Copyright) &&
                                             x.Trademark.Equals(y.Trademark) &&
                                             x.ProductName.Equals(y.ProductName) &&
                                             x.ProductVersion.Equals(y.ProductVersion) &&
                                             x.ProcessorArchitecture.Equals(y.ProcessorArchitecture) &&
                                             x.Bitness.Equals(y.Bitness) &&
                                             x.AuthentiCodeCertificateThumbprint.Equals(y.AuthentiCodeCertificateThumbprint);
        }

        public int GetHashCode(FileMetadata obj)
        {
            return obj.GetHashCode();
        }
    }
}