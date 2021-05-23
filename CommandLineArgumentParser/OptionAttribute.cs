using System;

namespace CommandLineArgumentParser
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class OptionAttribute : Attribute
    {
        public string Name { get; }
        public Type Type { get; }

        public OptionAttribute(string name, Type type)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} can't be null or empty");
            if (type is null)
                throw new ArgumentNullException(nameof(type), $"{nameof(type)} can't be null");

            Name = name;
            Type = type;
        }
    }
}