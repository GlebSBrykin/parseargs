using System;

namespace CommandLineArgumentParser
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class OptionAttribute : Attribute
    {
        public string Name { get; }

        public OptionAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} can't be null or empty");

            Name = name;
        }
    }
}
