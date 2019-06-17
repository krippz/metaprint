using System;
using System.Collections.Generic;
using Metadata;
using Readers;
using Print;

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