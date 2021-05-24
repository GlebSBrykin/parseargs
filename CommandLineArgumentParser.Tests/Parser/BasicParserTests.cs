using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CommandLineArgumentParser.Tests.Parser
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "UnassignedField.Compiler")]
    [TestFixture]
    public class BasicParserTests
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

        [Parsable]
        private class ParsableAnyTypeWithSeveralMembersNotMarkedAsFlagsOrOptions
        {
            public bool Field1;
            public int Field2;
            public float Field3;
            public string Field4;
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

        [Test]
        public void Expect_NoException_And_NotNullInstance_When_GenericParameterIsParsable_And_NoMembersDefined()
        {
            Assert.IsNotNull(new Parser<ParsableAnyEmptyType>(Array.Empty<string>()).Parse(),
                "No exception and not null instance expected when T generic parameter was parsable and no members were defined.");
        }

        [Test]
        public void Expect_NoException_And_InstanceWithDefaultMemberValues_When_GenericParameterIsParsable_And_NoMembersDefinedAsFlagsOrOptions()
        {
            var instance = new Parser<ParsableAnyTypeWithSeveralMembersNotMarkedAsFlagsOrOptions>(Array.Empty<string>()).Parse();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(instance.Field1, default(bool),
                    $"No exception and not null instance expected with {nameof(instance.Field1)} equal to {default(bool)} when T generic parameter was parsable and no members were defined as flags or options.");
                Assert.AreEqual(instance.Field2, default(int),
                    $"No exception and not null instance expected with {nameof(instance.Field2)} equal to {default(int)} when T generic parameter was parsable and no members were defined as flags or options.");
                Assert.AreEqual(instance.Field3, default(float),
                    $"No exception and not null instance expected with {nameof(instance.Field3)} equal to {default(float)} when T generic parameter was parsable and no members were defined as flags or options.");
                Assert.AreEqual(instance.Field4, default(string),
                    $"No exception and not null instance expected with {nameof(instance.Field4)} equal to {default(string)} when T generic parameter was parsable and no members were defined as flags or options.");
            });
        }
    }
}