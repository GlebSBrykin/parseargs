# parseargs

## Description

[![CI](https://github.com/alvinseville7cf/parseargs/actions/workflows/ci.yml/badge.svg)](https://github.com/alvinseville7cf/parseargs/actions/workflows/ci.yml) [![GitHub release](https://img.shields.io/github/release/Naereen/StrapDown.js.svg)](https://github.com/alvinseville7cf/parseargs/releases/)

Command line argument parser for .NET based on attributes.

## Installation
You can download parseargs from [GitHub releases](https://github.com/alvinseville7cf/parseargs/releases) as .dll or install it as [NuGet package](https://www.nuget.org/packages/CommandLineArgumentParser/).

## Introduction

parseargs introduces some terminology such as flag and option:
- flag - command line switch without explicitly passed value with bool type (`--is-enabled` for example)
- option - command line switch with value with bool|int|float|string type (`--foreground red` for example)

Each flag/option must correspond to some field/property to be recognized as such:

```cs
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
```

Here we define two options `-f`|`--first` and `-s`|`--second` where passed as command line arguments numbers are stored. Note that all parsed info is stored in Store (in our case) class. For example if we run out program with Store class with the following switches:

```
--first 4 --second 5
```

then First field is 4 and second one is 5 after parsing args array in Main method. Missed options mean default values assigned to corresponding fields/properties.

### Classes

There are several attributes to mark fields/properties as flags/options:
- `CommandLineArgumentParser.ParsableAttribute()` - marks class which stores command line argument parsing results (it is mandatory attribute for types which store parse results)
- `CommandLineArgumentParser.FlagAttribute(name)` - marks type member which stores flag bool value
- `CommandLineArgumentParser.OptionAttribute(name)` - marks type member which stores option value

You have to invoke Parse method of `CommandLineArgumentParser.Parser<StorageType>(args)` class to fill StorageType type fields/properties with flag/option values.

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
