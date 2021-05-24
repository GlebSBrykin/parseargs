using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CommandLineArgumentParser.Tests.Parser
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "UnassignedField.Compiler")]
    [TestFixture]
    public static class BasicTests
    {
        private class AnyType
        {
        }

        private class EmptyNonParsable
        {
        }

        [Parsable]
        private class EmptyParsable
        {
        }

        [Parsable]
        private class NonEmptyParsable
        {
            public bool Field1;
            public int Field2;
            public float Field3;
            public string Field4;
        }

        [Test]
        public static void Expect_ArgumentNullException_When_ArgsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Parser<AnyType>(null),
                $"{nameof(ArgumentNullException)} expected when args parameter was null.");
        }

        [Test]
        public static void Expect_NoException_When_ArgsIsEmpty()
        {
            Assert.DoesNotThrow(() => new Parser<AnyType>(Array.Empty<string>()),
                "No exception expected when args parameter was not null.");
        }

        [Test]
        public static void Expect_ArgumentException_When_TIsNotParsable()
        {
            Assert.Throws<ArgumentException>(() => new Parser<EmptyNonParsable>(Array.Empty<string>()).Parse(),
                $"{nameof(ArgumentException)} expected when T was not parsable.");
        }

        [Test]
        public static void Expect_NoException_When_TIsParsable()
        {
            Assert.DoesNotThrow(() => new Parser<EmptyParsable>(Array.Empty<string>()).Parse(),
                "No exception expected when T was parsable.");
        }

        [Test]
        public static void Expect_NoException_And_NotNullInstance_When_TIsParsable_And_NoMembersDefined()
        {
            Assert.IsNotNull(new Parser<EmptyParsable>(Array.Empty<string>()).Parse(),
                "No exception and not null instance expected when T was parsable and no members were defined.");
        }

        [Test]
        public static void Expect_NoException_And_DefaultInstance_When_TIsParsable_And_NoFlagsAndOptionsDefined()
        {
            var instance = new Parser<NonEmptyParsable>(Array.Empty<string>()).Parse();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(instance.Field1, default(bool),
                    $"No exception and not null instance with {nameof(instance.Field1)} equal to {default(bool)} expected when T was parsable and no flags or options defined.");
                Assert.AreEqual(instance.Field2, default(int),
                    $"No exception and not null instance with {nameof(instance.Field2)} equal to {default(int)} expected when T was parsable and no flags or options defined.");
                Assert.AreEqual(instance.Field3, default(float),
                    $"No exception and not null instance with {nameof(instance.Field3)} equal to {default(float)} expected when T was parsable and no flags or options defined.");
                Assert.AreEqual(instance.Field4, default(string),
                    $"No exception and not null instance with {nameof(instance.Field4)} equal to {default(string)} expected when T was parsable and no flags or options defined.");
            });
        }
    }
}