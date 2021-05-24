using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CommandLineArgumentParser.Tests.Parser
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "UnassignedField.Compiler")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [TestFixture]
    public class SeparateMembersWithSeveralAttributesTests
    {
        [Parsable]
        private class WithFieldMarkedAsFlagTwice
        {
            [Flag(FlagName)]
            [Flag(SecondFlagName)]
            public bool Field;
        }

        [Parsable]
        private class WithFieldMarkedAsOptionTwice
        {
            [Option(OptionName)]
            [Option(SecondOptionName)]
            public int Field;
        }

        [Parsable]
        private class WithPropertyMarkedAsFlagTwice
        {
            [Flag(FlagName)]
            [Flag(SecondFlagName)]
            public bool Property { get; set; }
        }

        [Parsable]
        private class WithPropertyMarkedAsOptionTwice
        {
            [Option(OptionName)]
            [Option(SecondOptionName)]
            public int Property { get; set; }
        }

        [Parsable]
        private class WithFieldMarkedAsFlagAndOption
        {
            [Flag(FlagName)]
            [Option(OptionName)]
            public bool Field;
        }

        [Parsable]
        private class WithPropertyMarkedAsFlagAndOption
        {
            [Flag(FlagName)]
            [Option(OptionName)]
            public bool Property { get; set; }
        }

        [Parsable]
        private class WithFieldMarkedAsFlagWithDuplicatingNames
        {
            [Flag(FlagName)]
            [Flag(FlagName)]
            public bool Field;
        }

        [Parsable]
        private class WithFieldMarkedAsOptionWithDuplicatingNames
        {
            [Option(OptionName)]
            [Option(OptionName)]
            public bool Field;
        }

        [Parsable]
        private class WithPropertyMarkedAsFlagWithDuplicatingNames
        {
            [Flag(FlagName)]
            [Flag(FlagName)]
            public bool Property { get; set; }
        }

        [Parsable]
        private class WithPropertyMarkedAsOptionWithDuplicatingNames
        {
            [Option(OptionName)]
            [Option(OptionName)]
            public bool Property { get; set; }
        }

        [TestCase(FlagName)]
        [TestCase(SecondFlagName)]
        public void Expect_NoException_And_FilledInstanceWithField_When_TIsParsable_And_UsedFlagDefined(string name)
        {
            var instance = new Parser<WithFieldMarkedAsFlagTwice>(new [] { name }).Parse();

            Assert.AreEqual(instance.Field, true,
                $"No exception and not null instance with {nameof(instance.Field)} equal to {true} expected when T was parsable and used flag defined.");
        }

        [TestCase(OptionName)]
        [TestCase(SecondOptionName)]
        public void Expect_NoException_And_FilledInstanceWithField_When_TIsParsable_And_UsedOptionDefined(string name)
        {
            var instance = new Parser<WithFieldMarkedAsOptionTwice>(new [] { name, OptionValue.ToString() }).Parse();

            Assert.AreEqual(instance.Field, OptionValue,
                $"No exception and not null instance with {nameof(instance.Field)} equal to {OptionValue} expected when T was parsable and used option defined.");
        }


        [TestCase(FlagName)]
        [TestCase(SecondFlagName)]
        public void Expect_NoException_And_FilledInstanceWithProperty_When_TIsParsable_And_UsedFlagDefined(string name)
        {
            var instance = new Parser<WithPropertyMarkedAsFlagTwice>(new [] { name }).Parse();

            Assert.AreEqual(instance.Property, true,
                $"No exception and not null instance with {nameof(instance.Property)} equal to {true} expected when T was parsable and used flag defined.");
        }

        [TestCase(OptionName)]
        [TestCase(SecondOptionName)]
        public void Expect_NoException_And_FilledInstanceWithProperty_When_TIsParsable_And_UsedOptionDefined(string name)
        {
            var instance = new Parser<WithPropertyMarkedAsOptionTwice>(new [] { name, OptionValue.ToString() }).Parse();

            Assert.AreEqual(instance.Property, OptionValue,
                $"No exception and not null instance with {nameof(instance.Property)} equal to {OptionValue} expected when T was parsable and used option defined.");
        }

        [Test]
        public void Expect_ArgumentException_When_TIsParsable_And_WrongAttributesDefined()
        {
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() => new Parser<WithFieldMarkedAsFlagAndOption>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and wrong attributes defined.");
                Assert.Throws<ArgumentException>(() => new Parser<WithPropertyMarkedAsFlagAndOption>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and wrong attributes defined.");

            });
        }

        [Test]
        public void Expect_ArgumentException_When_TIsParsable_And_DuplicatingFlagDefined()
        {
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() => new Parser<WithFieldMarkedAsFlagWithDuplicatingNames>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and duplicating flag defined.");
                Assert.Throws<ArgumentException>(() => new Parser<WithPropertyMarkedAsFlagWithDuplicatingNames>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and duplicating flag defined.");
            });
        }

        [Test]
        public void Expect_ArgumentException_When_TIsParsable_And_DuplicatingOptionDefined()
        {
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() => new Parser<WithFieldMarkedAsOptionWithDuplicatingNames>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and duplicating option defined.");
                Assert.Throws<ArgumentException>(() => new Parser<WithPropertyMarkedAsOptionWithDuplicatingNames>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and duplicating option defined.");
            });
        }

        private const string FlagName = "-f";
        private const string SecondFlagName = "-f2";
        private const string OptionName = "-o";
        private const string SecondOptionName = "-o2";

        private const int OptionValue = 10;
    }
}