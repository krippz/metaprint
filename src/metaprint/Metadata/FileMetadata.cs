using System.Collections.Generic;
using System.Text;

namespace Metadata
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
        public static FileMetadata Empty => new FileMetadata();
        public FileMetadata()
        {
            this.FileName = string.Empty;
            this.InternalName = string.Empty;
            this.FileVersion = string.Empty;
            this.CompanyName = string.Empty;
            this.Copyright = string.Empty;
            this.Trademark = string.Empty;
            this.ProductName = string.Empty;
            this.ProductVersion = string.Empty;
            this.ProcessorArchitecture = string.Empty;
            this.Bitness = string.Empty;
        }
    }

    public class FileMetadataComparer : IEqualityComparer<FileMetadata>
    {
        public bool Equals(FileMetadata x, FileMetadata y)
        {
            return x.FileName.Equals(y.FileName) &&
                    x.InternalName.Equals(y.InternalName) &&
                    x.FileVersion.Equals(y.FileVersion) &&
                    x.CompanyName.Equals(y.CompanyName) &&
                    x.Copyright.Equals(y.Copyright) &&
                    x.Trademark.Equals(y.Trademark) &&
                    x.ProductName.Equals(y.ProductName) &&
                    x.ProductVersion.Equals(y.ProductVersion) &&
                    x.ProcessorArchitecture.Equals(y.ProcessorArchitecture) &&
                    x.Bitness.Equals(y.Bitness);
        }

        public int GetHashCode(FileMetadata obj)
        {
            return obj.GetHashCode();
        }
    }

}