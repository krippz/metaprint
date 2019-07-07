using System;
using System.Collections.Generic;
using System.Linq;
using metaprint.Metadata;
using metaprint.Readers;

namespace metaprint.Print
{
    public class Printer : IPrinter
    {
        private readonly IReader _reader;
        private readonly bool _verbose;
        private const int Width = 50;
        private static string Separator => new string('-', Width);
        public Printer(IReader reader, bool verbose = false)
        {
            _reader = reader;
            _verbose = verbose;
        }

        public void PrettyPrint(IEnumerable<string> files)
        {
            var builder = new FileMetadataBuilder(files, _reader);
            var director = new FileMetadataInfoDirector(builder);
            director.BuildFileMetadataInfo(_verbose);

            var info = builder.GetMetadata();
            Wrap(info);

            Console.Write(info);
        }

        private static void Wrap(FileMetadataInfo data)
        {
            var props = data.GetType().GetProperties().Where(x => x.CustomAttributes.Any(attr => attr.AttributeType == typeof(WrapPrettyAttribute)));

            foreach (var item in props)
            {
                var tail = Separator;
                const int space = 15;
                var leftAlign = 55;
                var formatString = "{0,-55}{1,-" + space + "}{2," + leftAlign + "}";

                if (item.Name.Length > space)
                {
                    var overflow = Math.Abs(space - item.Name.Length);
                    var adjustedSpace = Math.Abs(space - overflow);
                    tail = Separator.Remove(0, overflow + adjustedSpace);

                    leftAlign -= overflow;
                    formatString = "{0,-55}{1,-" + adjustedSpace + "}{2," + leftAlign + "}";
                }

                var header = string.Format(formatString, Separator, item.Name, tail);
                var footer = new string('-', header.Length);

                var value = item.GetValue(data, null)?.ToString();

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                
                var result = $"{header}{Environment.NewLine}{value}{Environment.NewLine}{footer}";
                item.SetValue(data, result);

            }
        }
    }
}