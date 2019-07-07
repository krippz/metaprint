using System;
using CommandLine;
using metaprint.Config;
using metaprint.Extensions;
using metaprint.Print;
using metaprint.Readers;

namespace Metaprint
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var parser = new Parser(configuration => configuration.HelpWriter = Console.Out);
            parser.ParseArguments<Options>(args)
                .WithParsed(RunPrinter);
        }

        private static void RunPrinter(Options o)
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