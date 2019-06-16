using Xunit;

namespace metaprint.test
{
    public class FileMetadataTests
    {
        [Fact]
        public void ShouldBeSameForNewAndEmpty()
        {
            var expected = new FileMetadata();
            var actual = FileMetadata.Empty;

            Assert.Equal(expected, actual, new FileMetadataComparer());
        }
    }
}