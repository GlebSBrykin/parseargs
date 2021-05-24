using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CommandLineArgumentParser.Tests.Parser
{
    [SuppressMessage("ReSharper", "UnassignedField.Compiler")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [TestFixture]
    public class DifferentMembersWithSingleAttributesTests
    {
        [Parsable]
        private class WithFieldsMarkedAsFlagWithDublicatingNames
        {
            [Flag(FlagName)]
            public bool Field;

            [Flag(FlagName)]
            public bool Field2;
        }

        [Parsable]
        private class WithFieldsMarkedAsOptionWithDublicatingNames
        {
            [Option(OptionName)]
            public bool Field;

            [Option(OptionName)]
            public bool Field2;
        }

        [Parsable]
        private class WithPropertiesMarkedAsFlagWithDublicatingNames
        {
            [Flag(FlagName)]
            public bool Property { get; set; }

            [Flag(FlagName)]
            public bool Property2 { get; set; }
        }

        [Parsable]
        private class WithPropertiesMarkedAsOptionWithDublicatingNames
        {
            [Option(OptionName)]
            public bool Property { get; set; }

            [Option(OptionName)]
            public bool Property2 { get; set; }
        }

        [Test]
        public void Expect_ArgumentException_When_TIsParsable_And_DublicatingFlagsDefined()
        {
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() => new Parser<WithFieldsMarkedAsFlagWithDublicatingNames>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and dublicating flags defined.");
                Assert.Throws<ArgumentException>(() => new Parser<WithPropertiesMarkedAsFlagWithDublicatingNames>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and dublicating flags defined.");
            });
        }

        [Test]
        public void Expect_ArgumentException_When_TIsParsable_And_DublicatingOptionsDefined()
        {
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() => new Parser<WithFieldsMarkedAsOptionWithDublicatingNames>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and dublicating options defined.");
                Assert.Throws<ArgumentException>(() => new Parser<WithPropertiesMarkedAsOptionWithDublicatingNames>(Array.Empty<string>()).Parse(),
                    $"{nameof(ArgumentException)} exception expected when T was parsable and dublicating options defined.");
            });
        }

        private const string FlagName = "-f";
        private const string OptionName = "-o";
    }
}