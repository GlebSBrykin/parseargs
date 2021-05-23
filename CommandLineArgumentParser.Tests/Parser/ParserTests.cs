using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CommandLineArgumentParser.Tests.Parser
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [TestFixture]
    public class ParserTests
    {
        private class AnyType
        {
        }

        private class NotParsableAnyEmptyType
        {
        }

        [Parsable]
        private class ParsableAnyEmptyType
        {
        }

        [Test]
        public void Expect_ArgumentNullException_When_ArgsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Parser<AnyType>(null),
                $"{nameof(ArgumentNullException)} expected when args parameter was null.");
        }

        [Test]
        public void Expect_NoException_When_ArgsIsEmpty()
        {
            Assert.DoesNotThrow(() => new Parser<AnyType>(Array.Empty<string>()),
                "No exception expected when args parameter was not null.");
        }

        [Test]
        public void Expect_ArgumentException_When_GenericParameterIsNotParsable()
        {
            Assert.Throws<ArgumentException>(() => new Parser<NotParsableAnyEmptyType>(Array.Empty<string>()).Parse(),
                $"{nameof(ArgumentException)} expected when T generic parameter was not parsable.");
        }

        [Test]
        public void Expect_NoException_When_GenericParameterIsParsable()
        {
            Assert.DoesNotThrow(() => new Parser<ParsableAnyEmptyType>(Array.Empty<string>()).Parse(),
                "No exception expected when T generic parameter was parsable.");
        }
    }
}