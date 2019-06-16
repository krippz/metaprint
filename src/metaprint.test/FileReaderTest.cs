using Xunit;

namespace metaprint.test
{
    public class FileReaderTests
    {
        [Fact]
        public void ShouldReturnFileNameForNonNetFile()
        {

            var reader = new FileReader(new Settings());
            var expected = "metaprint.test.runtimeconfig.json";

            var actual = reader.Read("./metaprint.test.runtimeconfig.json");

            Assert.Equal(expected, actual.FileName);
        }
        [Fact]
        public void ShouldReturnNAVersionForNonNetFile()
        {
            var reader = new FileReader(new Settings());
            var expected = "N/A";

            var actual = reader.Read("./metaprint.test.runtimeconfig.json");

            Assert.Equal(expected, actual.FileVersion);
        }

        [Fact]
        public void ShouldReturnEmptyFileMetadataWhenFileDowsNotExist()
        {
            var reader = new FileReader(new Settings());
            var expected = new FileMetadata();

            var actual = reader.Read("");
            Assert.Equal(expected, actual, new FileMetadataComparer());
        }
    }
}