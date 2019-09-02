using System.Collections.Generic;
using metaprint.Metadata;

namespace metaprint.Readers
{
    public interface IReader
    {
        IEnumerable<FileMetadata> Read(IEnumerable<string> paths);
    }
}