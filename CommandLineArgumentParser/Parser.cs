using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLineArgumentParser
{
    public class Parser<T>
        where T : new()
    {
        public IEnumerable<string> Args { get; }

        public Parser(IEnumerable<string> args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args), $"{nameof(args)} can't be null");

            Args = args;
        }

        public T Parse()
        {
            T result = new();
            Type type = result.GetType();
            if (!Attribute.GetCustomAttributes(type).Any(attr => attr is ParsableAttribute))
                throw new ArgumentException($"{nameof(T)} must have ParsableAttribute", nameof(T));

            FieldInfo[] fields = type.GetFields();
            PropertyInfo[] properties = type.GetProperties();

            try
            {
                foreach (var field in fields)
                    FieldHasCorrectAttributes(field);
                foreach (var property in properties)
                    PropertyHasCorrectAttributes(property);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"field/property have incorrect {nameof(FlagAttribute)} and (or) {nameof(OptionAttribute)} attributes", e);
            }

            return result;
        }

        private static void FieldHasCorrectAttributes(FieldInfo fieldInfo)
        {
            IEnumerable<FlagAttribute> flagAttributes = fieldInfo.GetCustomAttributes<FlagAttribute>();
            IEnumerable<OptionAttribute> optionAttributes = fieldInfo.GetCustomAttributes<OptionAttribute>();

            if (flagAttributes.Any() && optionAttributes.Any())
                throw new ArgumentException($"{fieldInfo.Name} field can't have both {nameof(FlagAttribute)} and {nameof(OptionAttribute)} attributes", nameof(fieldInfo));

            if (flagAttributes.Any())
            {
                IEnumerable<string> names = flagAttributes.Select(attr => attr.Name);
                if (names.Distinct().Count() != names.Count())
                    throw new ArgumentException($"{fieldInfo.Name} field can't have {nameof(FlagAttribute)} with dublicating names", nameof(fieldInfo));
            }
            else if (optionAttributes.Any())
            {
                IEnumerable<string> names = optionAttributes.Select(attr => attr.Name);
                if (names.Distinct().Count() != names.Count())
                    throw new ArgumentException($"{fieldInfo.Name} field can't have {nameof(OptionAttribute)} with dublicating names", nameof(fieldInfo));
                if (optionAttributes.Select(attr => attr.Type).Distinct().Count() > 1)
                    throw new ArgumentException($"{fieldInfo.Name} field can't have {nameof(OptionAttribute)} with different types", nameof(fieldInfo));
            }
        }

        private static void PropertyHasCorrectAttributes(PropertyInfo propertyInfo)
        {
            IEnumerable<FlagAttribute> flagAttributes = propertyInfo.GetCustomAttributes<FlagAttribute>();
            IEnumerable<OptionAttribute> optionAttributes = propertyInfo.GetCustomAttributes<OptionAttribute>();

            if (flagAttributes.Any() && optionAttributes.Any())
                throw new ArgumentException($"{propertyInfo.Name} field can't have both {nameof(FlagAttribute)} and {nameof(OptionAttribute)} attributes", nameof(propertyInfo));

            if (flagAttributes.Any())
            {
                IEnumerable<string> names = flagAttributes.Select(attr => attr.Name);
                if (names.Distinct().Count() != names.Count())
                    throw new ArgumentException($"{propertyInfo.Name} field can't have {nameof(FlagAttribute)} with dublicating names", nameof(propertyInfo));
            }
            else if (optionAttributes.Any())
            {
                IEnumerable<string> names = optionAttributes.Select(attr => attr.Name);
                if (names.Distinct().Count() != names.Count())
                    throw new ArgumentException($"{propertyInfo.Name} field can't have {nameof(OptionAttribute)} with dublicating names", nameof(propertyInfo));
                if (optionAttributes.Select(attr => attr.Type).Distinct().Count() > 1)
                    throw new ArgumentException($"{propertyInfo.Name} field can't have {nameof(OptionAttribute)} with different types", nameof(propertyInfo));
            }
        }
    }
}
