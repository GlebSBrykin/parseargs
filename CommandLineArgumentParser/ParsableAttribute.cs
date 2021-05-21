using System;

namespace CommandLineArgumentParser
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class ParsableAttribute : Attribute
    {
    }
}
