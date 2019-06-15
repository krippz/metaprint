using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace versions
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser(configuration => configuration.HelpWriter = Console.Out);
            var result = parser.ParseArguments<Options>(args)
                               .WithParsed(options => RunPrinter(options));
        }
        static void RunPrinter(Options o)
        {

            var reader = new FileReader(o.Extensions);
            var printer = new Printer(reader, o.Verbose);

            if (!o.Files.IsNullOrEmpty())
            {
                printer.PrettyPrint(o.Files);
            }
            else if (!o.Directories.IsNullOrEmpty())
            {
                printer.PrettyPrint(o.Directories);
            }
        }
    }
}
