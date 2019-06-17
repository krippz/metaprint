using System.Collections.Generic;
using CommandLine;

namespace Config
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Prints verbose versions")]
        public bool Verbose { get; set; }

        [Option('e', "ext", Separator = ':', Required = false, HelpText = "What extensions do you want to check")]
        public IEnumerable<string> Extensions { get; set; }

        [Option('r', "read", Required = false, HelpText = "Files to be read")]
        public IEnumerable<string> Files { get; set; }

        [Option('d', "directories", Required = true, HelpText = "Run against all files in folder(s)")]
        public IEnumerable<string> Directories { get; set; }

        [Option('c', "company", Separator = ':', Required = false, HelpText = "Filter by only these companys")]
        public IEnumerable<string> Companys { get; set; }
    }
}