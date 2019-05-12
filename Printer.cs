using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;


public interface IPrinter
{
    void PrettyPrint(IEnumerable<string> files);
}
public class Printer : IPrinter
{
    private IReader reader;
    private bool verbose;


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

        Console.Write(info);
    }
}

public static class PrettyExtension
{
    public static string Truncate(this string value, int maxChars)
    {
        if (value != null)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars);
        }
        else
        {
            return string.Empty;
        }
    }
}
