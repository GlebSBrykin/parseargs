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
            IList<FlagAttribute> flagAttributes = fieldInfo.GetCustomAttributes<FlagAttribute>().ToList();
            IList<OptionAttribute> optionAttributes = fieldInfo.GetCustomAttributes<OptionAttribute>().ToList();

            if (flagAttributes.Count > 0 && optionAttributes.Count > 0)
                throw new ArgumentException($"{fieldInfo.Name} field can't have both {nameof(FlagAttribute)} and {nameof(OptionAttribute)} attributes", nameof(fieldInfo));

            if (flagAttributes.Count > 0)
            {
                IEnumerable<string> names = flagAttributes.Select(attr => attr.Name);
                if (names.Distinct().Count() != names.Count())
                    throw new ArgumentException($"{fieldInfo.Name} field can't have {nameof(FlagAttribute)} with dublicating names", nameof(fieldInfo));
            }
            else if (optionAttributes.Count > 0)
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
            IList<FlagAttribute> flagAttributes = propertyInfo.GetCustomAttributes<FlagAttribute>().ToList();
            IList<OptionAttribute> optionAttributes = propertyInfo.GetCustomAttributes<OptionAttribute>().ToList();

            if (flagAttributes.Count > 0 && optionAttributes.Count > 0)
                throw new ArgumentException($"{propertyInfo.Name} field can't have both {nameof(FlagAttribute)} and {nameof(OptionAttribute)} attributes", nameof(propertyInfo));

            if (flagAttributes.Count > 0)
            {
                IEnumerable<string> names = flagAttributes.Select(attr => attr.Name);
                if (names.Distinct().Count() != names.Count())
                    throw new ArgumentException($"{propertyInfo.Name} field can't have {nameof(FlagAttribute)} with dublicating names", nameof(propertyInfo));
            }
            else if (optionAttributes.Count > 0)
            {
                IEnumerable<string> names = optionAttributes.Select(attr => attr.Name);
                if (names.Distinct().Count() != names.Count())
                    throw new ArgumentException($"{propertyInfo.Name} field can't have {nameof(OptionAttribute)} with dublicating names", nameof(propertyInfo));
                if (optionAttributes.Select(attr => attr.Type).Distinct().Count() > 1)
                    throw new ArgumentException($"{propertyInfo.Name} field can't have {nameof(OptionAttribute)} with different types", nameof(propertyInfo));
            }
        }

        private static IDictionary<string, SupportedType> CreateDictionaryForFields(IEnumerable<FieldInfo> fieldInfos)
        {
            IDictionary<string, SupportedType> result = new Dictionary<string, SupportedType>();
            foreach (var fieldInfo in fieldInfos)
            {
                KeyValuePair<string, SupportedType> pair = CreateKeyValuePairForField(fieldInfo);
                result.Add(pair.Key, pair.Value);
            }
            return result;
        }

        private static IDictionary<string, SupportedType> CreateDictionaryForProperties(IEnumerable<PropertyInfo> propertyInfos)
        {
            IDictionary<string, SupportedType> result = new Dictionary<string, SupportedType>();
            foreach (var fieldInfo in propertyInfos)
            {
                KeyValuePair<string, SupportedType> pair = CreateKeyValuePairForProperty(fieldInfo);
                result.Add(pair.Key, pair.Value);
            }
            return result;
        }

        private static KeyValuePair<string, SupportedType> CreateKeyValuePairForField(FieldInfo fieldInfo)
        {
            IEnumerable<string> flagAttributeNames = fieldInfo.GetCustomAttributes<FlagAttribute>().Select(attr => attr.Name);

            IEnumerable<OptionAttribute> optionAttributes = fieldInfo.GetCustomAttributes<OptionAttribute>();
            IEnumerable<string> optionAttributeNames = optionAttributes.Select(attr => attr.Name);

            IDictionary<Type, SupportedType> typesToSupportedTypes = new Dictionary<Type, SupportedType>()
            {
                [typeof(string)] = SupportedType.String,
                [typeof(int)] = SupportedType.Int,
                [typeof(float)] = SupportedType.Float
            };

            if (flagAttributeNames.Any())
                return new KeyValuePair<string, SupportedType>(flagAttributeNames.First(), SupportedType.None);
            else if (optionAttributeNames.Any())
                return new KeyValuePair<string, SupportedType>(optionAttributeNames.First(), typesToSupportedTypes[optionAttributes.First().Type]);

            throw new ArgumentException($"field have incorrect {nameof(FlagAttribute)} and (or) {nameof(OptionAttribute)} attributes", nameof(fieldInfo));
        }

        private static KeyValuePair<string, SupportedType> CreateKeyValuePairForProperty(PropertyInfo propertyInfo)
        {
            IEnumerable<string> flagAttributeNames = propertyInfo.GetCustomAttributes<FlagAttribute>().Select(attr => attr.Name);

            IEnumerable<OptionAttribute> optionAttributes = propertyInfo.GetCustomAttributes<OptionAttribute>();
            IEnumerable<string> optionAttributeNames = optionAttributes.Select(attr => attr.Name);

            IDictionary<Type, SupportedType> typesToSupportedTypes = new Dictionary<Type, SupportedType>()
            {
                [typeof(string)] = SupportedType.String,
                [typeof(int)] = SupportedType.Int,
                [typeof(float)] = SupportedType.Float
            };

            if (flagAttributeNames.Any())
                return new KeyValuePair<string, SupportedType>(flagAttributeNames.First(), SupportedType.None);
            else if (optionAttributeNames.Any())
                return new KeyValuePair<string, SupportedType>(optionAttributeNames.First(), typesToSupportedTypes[optionAttributes.First().Type]);

            throw new ArgumentException($"field have incorrect {nameof(FlagAttribute)} and (or) {nameof(OptionAttribute)} attributes", nameof(propertyInfo));
        }
    }
}
