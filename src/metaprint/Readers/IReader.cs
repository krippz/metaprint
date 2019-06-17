using System.Collections.Generic;
using Metadata;

namespace Readers
{
    public interface IReader
    {
        IEnumerable<FileMetadata> Read(IEnumerable<string> paths);
        FileMetadata Read(string path);
    }
}