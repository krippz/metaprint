using System;
using System.Collections.Generic;
using Metadata;
using Readers;
using Print;
using System.Linq;

public class Printer : IPrinter
{
    private IReader reader;
    private bool verbose;
    private const int Width = 50;
    private string separator => new string('-', Width);
    public Printer(IReader reader, bool verbose = false)
    {
        this.reader = reader;
        this.verbose = verbose;
    }

    public void PrettyPrint(IEnumerable<string> files)
    {

        var builder = new FileMetadataBuilder(files, reader);
        var director = new FileMetadataInfoDirector(builder);
        director.BuildFileMetadataInfo(verbose);

        var info = builder.GetMetadata();
        Wrap(info);

        Console.Write(info);
    }

    private void Wrap(FileMetadataInfo data)
    {
        var props = data.GetType().GetProperties().Where(x => x.CustomAttributes.Any(attr => attr.AttributeType == typeof(WrapPrettyAttribute)));

        foreach (var item in props)
        {
            var tail = separator;
            var space = 15;
            var leftAlign = 55;
            var formatStrign = "{0,-55}{1,-" + space + "}{2," + leftAlign + "}";

            if (item.Name.Length > space)
            {
                var overflow = Math.Abs(space - item.Name.Length);
                var adjustedSpace = Math.Abs(space - overflow);
                tail = separator.Remove(0, overflow + adjustedSpace);

                leftAlign = leftAlign - overflow;
                formatStrign = "{0,-55}{1,-" + adjustedSpace + "}{2," + leftAlign + "}";
            }

            var header = string.Format(formatStrign, separator, item.Name, tail);
            var footer = new string('-', header.Length);

            var value = item.GetValue(data, null)?.ToString();
            if (!string.IsNullOrEmpty(value))
            {
                var result = $"{header}{System.Environment.NewLine}{value}{System.Environment.NewLine}{footer}";
                item.SetValue(data, result);
            }

        }
    }
}