using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CommandLineArgumentParser.Tests.Attributes
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [TestFixture]
    public class FlagAttributeTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void Expect_ArgumentNullException_When_NameIsNull(string name)
        {
            Assert.Throws<ArgumentNullException>(() => new FlagAttribute(name),
                $"{nameof(ArgumentNullException)} expected when name parameter was null or empty.");
        }

        [Test]
        public void Expect_NoException_When_NameIsNotNullAndNotEmpty()
        {
            Assert.DoesNotThrow(() => new FlagAttribute("--flag"),
                "No exception expected when name parameter was not null and not empty.");
        }
    }
}