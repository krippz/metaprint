using Xunit;
using metaprint.Metadata;
using metaprint.Readers;

namespace Metaprint.Test
{
    public class FileReaderTests
    {
        [Fact]
        public void ShouldReturnFileNameForNonNetFile()
        {
            const string expected = "metaprint.test.runtimeconfig.json";

            var actual = FileReader.Read("./metaprint.test.runtimeconfig.json");

            Assert.Equal(expected, actual.FileName);
        }

        [Fact]
        public void ShouldReturnNaVersionForNonNetFile()
        {

            const string expected = "N/A";

            var actual = FileReader.Read("./metaprint.test.runtimeconfig.json");

            Assert.Equal(expected, actual.FileVersion);
        }

        [Fact]
        public void ShouldReturnEmptyFileMetadataWhenFileDoesNotExist()
        {
            var expected = new FileMetadata();

            var actual = FileReader.Read("");
            Assert.Equal(expected, actual, new FileMetadataComparer());
        }
    }
}