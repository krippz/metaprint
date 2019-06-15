using System.Collections.Generic;
public interface IReader
{
    IEnumerable<FileMetadata> Read(IEnumerable<string> paths);
    FileMetadata Read(string path);
}





