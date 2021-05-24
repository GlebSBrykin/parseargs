using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CommandLineArgumentParser.Tests.Parser
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "UnassignedField.Compiler")]
    [TestFixture]
    public class SingularClassMembersParserTests
    {
        [Parsable]
        private class ParsableAnyTypeWithFieldMarkedAsFlag
        {
            [Flag(FlagName)]
            public bool Field;
        }

        [Parsable]
        private class ParsableAnyTypeWithFieldMarkedAsOption
        {
            [Option(OptionName)]
            public int Field;
        }

        [Parsable]
        private class ParsableAnyTypeWithPropertyMarkedAsFlag
        {
            [Flag(FlagName)]
            public bool Property { get; set; }
        }

        [Parsable]
        private class ParsableAnyTypeWithPropertyMarkedAsOption
        {
            [Option(OptionName)]
            public int Property { get; set; }
        }

        [Test]
        public void Expect_NoException_And_InstanceWithDefaultFieldValue_When_GenericParameterIsParsable_And_FieldMarkedAsFlag_And_NoFlagValueProvided()
        {
            var instance = new Parser<ParsableAnyTypeWithFieldMarkedAsFlag>(Array.Empty<string>()).Parse();

            Assert.AreEqual(instance.Field, default(bool),
                $"No exception and not null instance expected with {nameof(instance.Field)} equal to {default(bool)} when T generic parameter was parsable and field defined as flag without assigned value.");
        }

        [Test]
        public void Expect_NoException_And_InstanceWithAssignedFieldValue_When_GenericParameterIsParsable_And_FieldMarkedAsFlag_And_FlagValueProvided()
        {
            var instance = new Parser<ParsableAnyTypeWithFieldMarkedAsFlag>(new [] { FlagName }).Parse();

            Assert.AreEqual(instance.Field, true,
                $"No exception and not null instance expected with {nameof(instance.Field)} equal to {true} when T generic parameter was parsable and field defined as flag with assigned value.");
        }

        [Test]
        public void Expect_NoException_And_InstanceWithDefaultFieldValue_When_GenericParameterIsParsable_And_FieldMarkedAsOption_And_NoOptionValueProvided()
        {
            var instance = new Parser<ParsableAnyTypeWithFieldMarkedAsOption>(Array.Empty<string>()).Parse();

            Assert.AreEqual(instance.Field, default(int),
                $"No exception and not null instance expected with {nameof(instance.Field)} equal to {default(int)} when T generic parameter was parsable and field defined as option without assigned value.");
        }

        [Test]
        public void Expect_NoException_And_InstanceWithAssignedFieldValue_When_GenericParameterIsParsable_And_FieldMarkedAsOption_And_OptionValueProvided()
        {
            var instance = new Parser<ParsableAnyTypeWithFieldMarkedAsOption>(new [] { OptionName, OptionValue.ToString() }).Parse();

            Assert.AreEqual(instance.Field, OptionValue,
                $"No exception and not null instance expected with {nameof(instance.Field)} equal to {true} when T generic parameter was parsable and option defined as flag with assigned value.");
        }

        [Test]
        public void Expect_NoException_And_InstanceWithDefaultPropertyValue_When_GenericParameterIsParsable_And_PropertyMarkedAsFlag_And_NoFlagValueProvided()
        {
            var instance = new Parser<ParsableAnyTypeWithPropertyMarkedAsFlag>(Array.Empty<string>()).Parse();

            Assert.AreEqual(instance.Property, default(bool),
                $"No exception and not null instance expected with {nameof(instance.Property)} equal to {default(bool)} when T generic parameter was parsable and field defined as flag without assigned value.");
        }

        [Test]
        public void Expect_NoException_And_InstanceWithAssignedPropertyValue_When_GenericParameterIsParsable_And_PropertyMarkedAsFlag_And_FlagValueProvided()
        {
            var instance = new Parser<ParsableAnyTypeWithPropertyMarkedAsFlag>(new [] { FlagName }).Parse();

            Assert.AreEqual(instance.Property, true,
                $"No exception and not null instance expected with {nameof(instance.Property)} equal to {true} when T generic parameter was parsable and field defined as flag with assigned value.");
        }

        [Test]
        public void Expect_NoException_And_InstanceWithDefaultPropertyValue_When_GenericParameterIsParsable_And_PropertyMarkedAsOption_And_NoOptionValueProvided()
        {
            var instance = new Parser<ParsableAnyTypeWithPropertyMarkedAsOption>(Array.Empty<string>()).Parse();

            Assert.AreEqual(instance.Property, default(int),
                $"No exception and not null instance expected with {nameof(instance.Property)} equal to {default(int)} when T generic parameter was parsable and field defined as option without assigned value.");
        }

        [Test]
        public void Expect_NoException_And_InstanceWithAssignedPropertyValue_When_GenericParameterIsParsable_And_PropertyMarkedAsOption_And_OptionValueProvided()
        {
            var instance = new Parser<ParsableAnyTypeWithPropertyMarkedAsOption>(new [] { OptionName, OptionValue.ToString() }).Parse();

            Assert.AreEqual(instance.Property, OptionValue,
                $"No exception and not null instance expected with {nameof(instance.Property)} equal to {true} when T generic parameter was parsable and option defined as flag with assigned value.");
        }

        private const string FlagName = "-f";
        private const string OptionName = "-o";

        private const int OptionValue = 10;
    }
}