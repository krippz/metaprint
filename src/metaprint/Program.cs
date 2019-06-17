using System;
using CommandLine;
using Config;
using Extensions;
using Readers;

namespace Metaprint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = new Parser(configuration => configuration.HelpWriter = Console.Out);
            var result = parser.ParseArguments<Options>(args)
                                .WithParsed(options => RunPrinter(options));
        }
        public static void RunPrinter(Options o)
        {
            var settings = new Settings(o.Extensions);
            var reader = new FileReader(settings);
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