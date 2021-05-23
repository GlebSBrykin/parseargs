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
    }
}