# parseargs
Command line argument parser for .NET.

## Classes

- `CommandLineArgumentParser.ParsableAttribute()` - marks class which stores command line argument parsing results
- `CommandLineArgumentParser.FlagAttribute(name)` - marks type member which stores command line flag bool value
- `CommandLineArgumentParser.OptionAttribute(name, type)` - marks type member which stores command line option value
- `CommandLineArgumentParser.Parser(args)` - command line argument parser with `Parse` method to parse command line arguments passed as array

## Examples

```cs
[CommandLineArgumentParser.ParsableAttribute]
public class Store
{
  [CommandLineArgumentParser.FlagAttribute("-v")]
  [CommandLineArgumentParser.FlagAttribute("--version")]
  public bool IsVersion { get; }
  
  [CommandLineArgumentParser.FlagAttribute("-h")]
  [CommandLineArgumentParser.FlagAttribute("--help")]
  public bool IsHelp { get; }
  
  [CommandLineArgumentParser.OptionAttribute("-f", typeof(int))]
  [CommandLineArgumentParser.OptionAttribute("--first", typeof(int))]
  public int First { get; }
  
  [CommandLineArgumentParser.OptionAttribute("-s", typeof(int))]
  [CommandLineArgumentParser.OptionAttribute("--second", typeof(int))]
  public int Second { get; }
}
```
