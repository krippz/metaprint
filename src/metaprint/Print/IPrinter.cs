using System.Collections.Generic;

namespace metaprint.Print
{
    public interface IPrinter
    {
        void PrettyPrint(IEnumerable<string> files);
    }
}