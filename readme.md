# Metaprint
[![Build Status](https://dev.azure.com/klinnestjerna/metaprint/_apis/build/status/krippz.metaprint?branchName=master)](https://dev.azure.com/klinnestjerna/metaprint/_build/latest?definitionId=1&branchName=master)

Tired of clicking up the Explorer meta data?

Want to check each file in a folder for stuff like copyright or just to see what version a file has?

This little dotnet tool has you covered.



## Installation

You will need tho have the dotnet sdk installed, you can get it here [https://dotnet.microsoft.com/download/dotnet/5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

Since this runs as a dotnet tool the full SDK is needed.

Run the following command to set it up:

```sh
dotnet tool install metaprint -g
```

More info can be found here [https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install)

This will install it as a global tool, see the help to see what it can do

```Powershell
metaprint --help
```

The simple example with one file:
```
metaprint -f some-random.dll
```

Look at the files in a directory:
```
metaprint -d c:\data\files
```

The verbose example with a directory but just files with with ".1" extension:
```
metaprint -d c:\data\files -e ".1" -v
```

