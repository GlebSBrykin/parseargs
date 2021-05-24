using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CommandLineArgumentParser.Tests.Parser
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "UnassignedField.Compiler")]
    [TestFixture]
    public class SeparateMembersWithSingleAttributeTests
    {
        [Parsable]
        private class WithFieldMarkedAsFlag
        {
            [Flag(FlagName)]
            public bool Field;
        }

        [Parsable]
        private class WithFieldMarkedAsOption
        {
            [Option(OptionName)]
            public int Field;
        }

        [Parsable]
        private class WithPropertyMarkedAsFlag
        {
            [Flag(FlagName)]
            public bool Property { get; set; }
        }

        [Parsable]
        private class WithPropertyMarkedAsOption
        {
            [Option(OptionName)]
            public int Property { get; set; }
        }

        [Test]
        public void Expect_NoException_And_DefaultInstanceWithField_When_TIsParsable_And_UnusedFlagDefined()
        {
            var instance = new Parser<WithFieldMarkedAsFlag>(Array.Empty<string>()).Parse();

            Assert.AreEqual(instance.Field, default(bool),
                $"No exception and default instance with {nameof(instance.Field)} equal to {default(bool)} expected when T was parsable and unused flag defined.");
        }

        [Test]
        public void Expect_NoException_And_FilledInstanceWithField_When_TIsParsable_And_UsedFlagDefined()
        {
            var instance = new Parser<WithFieldMarkedAsFlag>(new [] { FlagName }).Parse();

            Assert.AreEqual(instance.Field, true,
                $"No exception and not null instance with {nameof(instance.Field)} equal to {true} expected when T was parsable and used flag defined.");
        }

        [Test]
        public void Expect_NoException_And_DefaultInstanceWithField_When_TIsParsable_And_UnusedOptionDefined()
        {
            var instance = new Parser<WithFieldMarkedAsOption>(Array.Empty<string>()).Parse();

            Assert.AreEqual(instance.Field, default(int),
                $"No exception and not null instance with {nameof(instance.Field)} equal to {default(int)} expected when T was parsable and unused option defined.");
        }

        [Test]
        public void Expect_NoException_And_FilledInstanceWithField_When_TIsParsable_And_UsedOptionDefined()
        {
            var instance = new Parser<WithFieldMarkedAsOption>(new [] { OptionName, OptionValue.ToString() }).Parse();

            Assert.AreEqual(instance.Field, OptionValue,
                $"No exception and not null instance with {nameof(instance.Field)} equal to {true} expected when T was parsable and used option defined.");
        }

        [Test]
        public void Expect_NoException_And_DefaultInstanceWithProperty_When_TIsParsable_And_UnusedFlagDefined()
        {
            var instance = new Parser<WithPropertyMarkedAsFlag>(Array.Empty<string>()).Parse();

            Assert.AreEqual(instance.Property, default(bool),
                $"No exception and not null instance with {nameof(instance.Property)} equal to {default(bool)} expected when T was parsable and unused flag defined.");
        }

        [Test]
        public void Expect_NoException_And_FilledInstanceWithProperty_When_TIsParsable_And_UsedFlagDefined()
        {
            var instance = new Parser<WithPropertyMarkedAsFlag>(new [] { FlagName }).Parse();

            Assert.AreEqual(instance.Property, true,
                $"No exception and not null instance with {nameof(instance.Property)} equal to {true} expected when T was parsable and used flag defined.");
        }

        [Test]
        public void Expect_NoException_And_DefaultInstanceWithProperty_When_TIsParsable_And_UnusedOptionDefined()
        {
            var instance = new Parser<WithPropertyMarkedAsOption>(Array.Empty<string>()).Parse();

            Assert.AreEqual(instance.Property, default(int),
                $"No exception and not null instance with {nameof(instance.Property)} equal to {default(int)} expected when T was parsable and unused option defined.");
        }

        [Test]
        public void Expect_NoException_And_FilledInstanceWithProperty_When_TIsParsable_And_UsedOptionDefined()
        {
            var instance = new Parser<WithPropertyMarkedAsOption>(new [] { OptionName, OptionValue.ToString() }).Parse();

            Assert.AreEqual(instance.Property, OptionValue,
                $"No exception and not null instance with {nameof(instance.Property)} equal to {true} expected when T was parsable and used option defined.");
        }

        private const string FlagName = "-f";
        private const string OptionName = "-o";

        private const int OptionValue = 10;
    }
}