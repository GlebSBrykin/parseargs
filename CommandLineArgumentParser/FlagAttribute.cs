using System;

namespace CommandLineArgumentParser
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class FlagAttribute : Attribute
    {
        public string Name { get; }

        public FlagAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} can't be null or empty");

            Name = name;
        }
    }
}
