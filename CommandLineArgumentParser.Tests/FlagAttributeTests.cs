using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CommandLineArgumentParser.Tests
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [TestFixture]
    public class FlagAttributeTests
    {
        [Test]
        public void Expect_ArgumentNullException_When_NameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FlagAttribute(null),
                $"{nameof(ArgumentNullException)} expected when name parameter was null.");
        }

        [Test]
        public void Expect_ArgumentNullException_When_NameIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new FlagAttribute(string.Empty),
                $"{nameof(ArgumentNullException)} expected when name parameter was empty.");
        }

        [Test]
        public void Expect_NoException_When_NameIsNotNullAndNotEmpty()
        {
            Assert.DoesNotThrow(() => new FlagAttribute("--flag"),
                "No exception expected when name parameter was not null and not empty.");
        }
    }
}