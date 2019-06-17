using System.Collections.Generic;

namespace Print
{
    public interface IPrinter
    {
        void PrettyPrint(IEnumerable<string> files);
    }
}