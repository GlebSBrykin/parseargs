using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLineArgumentParser
{
    public class Parser<T>
        where T : new()
    {
        public IList<string> Args { get; }

        public Parser(IList<string> args)
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

            FieldInfo[] fieldInfos = type.GetFields();
            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (var info in fieldInfos)
                FieldHasCorrectAttributes(info);
            foreach (var info in propertyInfos)
                PropertyHasCorrectAttributes(info);

            try
            {
                IDictionary<string, Type> nameToTypeMap = CreateDictionaryForAllMembers(fieldInfos, propertyInfos);

                for (int i = 0; i < Args.Count; i++)
                {
                    string flagOrOption = Args[i];
                    string flagOrOptionValue = i + 1 < Args.Count ? Args[i + 1] : string.Empty;

                    if (nameToTypeMap.TryGetValue(flagOrOption, out Type flagOrOptionType))
                        ParseFlagOrOption(ref i, result, flagOrOption, flagOrOptionValue, flagOrOptionType);
                    else
                        throw new ArgumentException($"{flagOrOption} flag/option is not supported", nameof(Args));
                }
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"field/property have incorrect {nameof(FlagAttribute)} and (or) {nameof(OptionAttribute)} attributes", e);
            }

            return result;
        }

        private static void ParseFlagOrOption(ref int i, T instance, string flagOrOption, string flagOrOptionValue, Type flagOrOptionType)
        {
            if (flagOrOptionType == typeof(bool))
            {
                if (bool.TryParse(flagOrOptionValue, out bool boolValue))
                {
                    PutValue(instance, flagOrOption, boolValue);
                    i++;
                }
                else
                    throw new ArgumentException($"{flagOrOption} option has {flagOrOptionType.Name} type but passed value has another type", nameof(flagOrOption));
            }
            else if (flagOrOptionType == typeof(int))
            {
                if (int.TryParse(flagOrOptionValue, out int intValue))
                {
                    PutValue(instance, flagOrOption, intValue);
                    i++;
                }
                else
                    throw new ArgumentException($"{flagOrOption} option has {flagOrOptionType.Name} type but passed value has another type", nameof(flagOrOption));
            }
            else if (flagOrOptionType == typeof(float))
            {
                if (int.TryParse(flagOrOptionValue, out int floatValue))
                {
                    PutValue(instance, flagOrOption, floatValue);
                    i++;
                }
                else
                    throw new ArgumentException($"{flagOrOption} option has {flagOrOptionType.Name} type but passed value has another type", nameof(flagOrOption));
            }
            else if (flagOrOptionType == typeof(string))
            {
                PutValue(instance, flagOrOption, flagOrOptionValue);
                i++;
            }
            else if (flagOrOptionType is null)
                PutValue(instance, flagOrOption, true);
            else
                throw new ArgumentException($"{flagOrOption} flag/option has unsopported type {flagOrOptionType.Name}", nameof(flagOrOption));
        }

        private static void PutValue(T instance, string name, object value)
        {
            Type type = instance.GetType();

            IEnumerable<FieldInfo> fieldInfos = type.GetFields().Where(field => field.GetCustomAttributes<FlagAttribute>().Any(attr => attr.Name == name) ||
                field.GetCustomAttributes<OptionAttribute>().Any(attr => attr.Name == name));
            if (fieldInfos.Any())
                fieldInfos.First().SetValue(instance, value);
            else
            {
                IEnumerable<PropertyInfo> propertyInfos = type.GetProperties().Where(property => property.GetCustomAttributes<FlagAttribute>().Any(attr => attr.Name == name) ||
                property.GetCustomAttributes<OptionAttribute>().Any(attr => attr.Name == name));
                if (propertyInfos.Any())
                    propertyInfos.First().SetValue(instance, value);
            }
        }

        private static void FieldHasCorrectAttributes(FieldInfo fieldInfo)
        {
            IEnumerable<FlagAttribute> flagAttributes = fieldInfo.GetCustomAttributes<FlagAttribute>();
            IEnumerable<OptionAttribute> optionAttributes = fieldInfo.GetCustomAttributes<OptionAttribute>();

            if (flagAttributes.Any() && optionAttributes.Any())
                throw new ArgumentException($"{fieldInfo.Name} field can't have both {nameof(FlagAttribute)} and {nameof(OptionAttribute)} attributes", nameof(fieldInfo));

            if (flagAttributes.Any())
            {
                if (ThereAreDublicatingNames(flagAttributes))
                    throw new ArgumentException($"{fieldInfo.Name} field can't have {nameof(FlagAttribute)} with dublicating names", nameof(fieldInfo));
                if (fieldInfo.FieldType != typeof(bool))
                    throw new ArgumentException($"{fieldInfo.Name} flag field can't have non-bool type", nameof(fieldInfo));
            }
            else if (optionAttributes.Any())
            {
                if (ThereAreDublicatingNames(optionAttributes))
                    throw new ArgumentException($"{fieldInfo.Name} field can't have {nameof(OptionAttribute)} with dublicating names", nameof(fieldInfo));
            }
        }

        private static void PropertyHasCorrectAttributes(PropertyInfo propertyInfo)
        {
            IEnumerable<FlagAttribute> flagAttributes = propertyInfo.GetCustomAttributes<FlagAttribute>();
            IEnumerable<OptionAttribute> optionAttributes = propertyInfo.GetCustomAttributes<OptionAttribute>();

            if (flagAttributes.Any() && optionAttributes.Any())
                throw new ArgumentException($"{propertyInfo.Name} property can't have both {nameof(FlagAttribute)} and {nameof(OptionAttribute)} attributes", nameof(propertyInfo));

            if (flagAttributes.Any())
            {
                if (ThereAreDublicatingNames(flagAttributes))
                    throw new ArgumentException($"{propertyInfo.Name} property can't have {nameof(FlagAttribute)} with dublicating names", nameof(propertyInfo));
                if (propertyInfo.GetMethod?.ReturnType != typeof(bool))
                    throw new ArgumentException($"{propertyInfo.Name} flag property can't have non-bool type or don't have getter", nameof(propertyInfo));
            }
            else if (optionAttributes.Any())
            {
                if (ThereAreDublicatingNames(optionAttributes))
                    throw new ArgumentException($"{propertyInfo.Name} property can't have {nameof(OptionAttribute)} with dublicating names", nameof(propertyInfo));
            }
        }

        private static bool ThereAreDublicatingNames(IEnumerable<FlagAttribute> attributes)
        {
            IEnumerable<string> names = attributes.Select(attr => attr.Name);
            return names.Distinct().Count() != names.Count();
        }

        private static bool ThereAreDublicatingNames(IEnumerable<OptionAttribute> attributes)
        {
            IEnumerable<string> names = attributes.Select(attr => attr.Name);
            return names.Distinct().Count() != names.Count();
        }

        private static IDictionary<string, Type> CreateDictionaryForAllMembers(IEnumerable<FieldInfo> fieldInfos, IEnumerable<PropertyInfo> propertyInfos)
        {
            IDictionary<string, Type> nameToTypeMap = new Dictionary<string, Type>();

            foreach (var info in fieldInfos)
            {
                foreach (var pair in CreateDictionaryForField(info))
                    if (!nameToTypeMap.TryAdd(pair.Key, pair.Value))
                        throw new ArgumentException($"There are two fields with identical name name {pair.Key} in {nameof(FlagAttribute)} and (or) {nameof(OptionAttribute)} attributes", nameof(fieldInfos));
            }

            foreach (var info in propertyInfos)
            {
                foreach (var pair in CreateDictionaryForProperty(info))
                    if (!nameToTypeMap.TryAdd(pair.Key, pair.Value))
                        throw new ArgumentException($"There are two fields/properties with identical name {pair.Key} in {nameof(FlagAttribute)} and (or) {nameof(OptionAttribute)} attributes", nameof(propertyInfos));
            }

            return nameToTypeMap;
        }

        private static IDictionary<string, Type> CreateDictionaryForField(FieldInfo info)
        {
            IEnumerable<string> flagNames = info.GetCustomAttributes<FlagAttribute>().Select(attr => attr.Name);
            IEnumerable<string> optionNames = info.GetCustomAttributes<OptionAttribute>().Select(attr => attr.Name);

            IDictionary<string, Type> nameToTypeMap = new Dictionary<string, Type>();
            if (flagNames.Any())
                foreach (var name in flagNames)
                    nameToTypeMap.Add(name, null);
            else if (optionNames.Any())
                foreach (var name in optionNames)
                    nameToTypeMap.Add(name, info.FieldType);

            return nameToTypeMap;
        }

        private static IDictionary<string, Type> CreateDictionaryForProperty(PropertyInfo info)
        {
            IEnumerable<string> flagNames = info.GetCustomAttributes<FlagAttribute>().Select(attr => attr.Name);
            IEnumerable<string> optionNames = info.GetCustomAttributes<OptionAttribute>().Select(attr => attr.Name);

            IDictionary<string, Type> nameToTypeMap = new Dictionary<string, Type>();
            if (flagNames.Any())
                foreach (var name in flagNames)
                    nameToTypeMap.Add(name, null);
            else if (optionNames.Any())
                foreach (var name in optionNames)
                    nameToTypeMap.Add(name, info.GetMethod?.ReturnType);

            return nameToTypeMap;
        }
    }
}
