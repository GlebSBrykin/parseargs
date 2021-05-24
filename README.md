# parseargs

Command line argument parser for .NET.

## Classes

- `CommandLineArgumentParser.ParsableAttribute()` - marks class which stores command line argument parsing results
- `CommandLineArgumentParser.FlagAttribute(name)` - marks type member which stores command line flag bool value
- `CommandLineArgumentParser.OptionAttribute(name)` - marks type member which stores command line option value
- `CommandLineArgumentParser.Parser(args)` - command line argument parser with `Parse` method to parse command line arguments passed as array

## Examples
### Argument sum

The following program accepts two options `-f`|`--first` and `-s`|`--second` where int numbers are stored and prints their sum:

```cs
using System;
using CommandLineArgumentParser;

namespace NugetTest
{
    internal static class Program
    {
        [Parsable]
        private class Store
        {
            [Option("-f")]
            [Option("--first")]
            public int First;

            [Option("-s")]
            [Option("--second")]
            public int Second;
        }

        private static void Main(string[] args)
        {
            var store = new Parser<Store>(args).Parse();
            Console.WriteLine($"{store.First} + {store.Second} = {store.First + store.Second}");
            Console.ReadLine();
        }
    }
}
```

Field type specifies option type.

### Flag checking

The following program accepts two flags (options without values) `-fa`|`--flag-a` and `-fb`|`--flag-b` and then checks which ones are passed:

```cs
using System;
using CommandLineArgumentParser;

namespace NugetTest
{
    internal static class Program
    {
        [Parsable]
        private class Store
        {
            [Flag("-fa")]
            [Flag("--flag-a")]
            public bool FlagA;

            [Flag("-fb")]
            [Flag("--flag-b")]
            public bool FlagB;
        }

        private static void Main(string[] args)
        {
            var store = new Parser<Store>(args).Parse();
            Console.WriteLine($"Is --flag-a enabled? {store.FlagA}");
            Console.WriteLine($"Is --flag-b enabled? {store.FlagB}");
            Console.ReadLine();
        }
    }
}
```
