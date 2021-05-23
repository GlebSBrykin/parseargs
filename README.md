# parseargs

Command line argument parser for .NET.

## Classes

- `CommandLineArgumentParser.ParsableAttribute()` - marks class which stores command line argument parsing results
- `CommandLineArgumentParser.FlagAttribute(name)` - marks type member which stores command line flag bool value
- `CommandLineArgumentParser.OptionAttribute(name)` - marks type member which stores command line option value
- `CommandLineArgumentParser.Parser(args)` - command line argument parser with `Parse` method to parse command line arguments passed as array

## Examples

Class to store parsed argument values:

```cs
[ParsableAttribute]
public class Store
{
  [FlagAttribute("-v")]
  [FlagAttribute("--version")]
  public bool IsVersion { get; }
  
  [FlagAttribute("-h")]
  [FlagAttribute("--help")]
  public bool IsHelp { get; }
  
  [OptionAttribute("-f")]
  [OptionAttribute("--first")]
  public int First { get; }
  
  [OptionAttribute("-s")]
  [OptionAttribute("--second")]
  public int Second { get; }
}
```

Main program:

```cs
private static void Main(string[] args)
{
  Parser<Store> parser = new Parser<Store>(args);
  Store store = parser.Parse();
}
```
