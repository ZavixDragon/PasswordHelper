using PasswordHelper;
using PasswordHelper.CharacterSuppliers;
using PasswordHelperTests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PasswordHelperTests
{
    public class PasswordGenerationTests
    {
        [Fact]
        [Trait("Category", "Unit")]
        public void PassedNoCharacterSuppliers_ArgumentInvalidThrown()
        {
            Assert.Throws<ArgumentException>(() => new PasswordGeneration(8, new List<ICharacters>()));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void GivenCharacterNSupplier_SuppliesPasswordWithAllNs()
        {
            var passwordGeneration = new PasswordGeneration(8, new List<ICharacters> { new TheCharacterN() });

            var password = passwordGeneration.Get();

            Assert.True(password.All(x => x.Equals('N')));
        }

        [Theory]
        [Trait("Category", "Unit")]
        [InlineData(1)]
        [InlineData(8)]
        [InlineData(32)]
        public void GivenACertainLength_SuppliesPasswordWithProperLength(int length)
        {
            var passwordGeneration = new PasswordGeneration(length, new List<ICharacters> { new TheCharacterN() });

            var password = passwordGeneration.Get();

            Assert.Equal(length, password.Count());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void GetPasswordFromGenerationTwice_SamePasswordReferenceReturned()
        {
            var passwordGeneration = new PasswordGeneration(8, new List<ICharacters> { new LowercaseLetters() });

            var password1 = passwordGeneration.Get();
            var password2 = passwordGeneration.Get();

            Assert.Equal(password1, password2);
        } 

        [Fact]
        [Trait("Category", "Unit")]
        public void GeneratePasswordWithTwoSuppliers_AlwaysUsesBothSuppliers()
        {
            var passwordGeneration = new PasswordGeneration(2, new List<ICharacters>
            {
                new TheCharacterN(),
                new LowercaseLetters()
            });

            var password = passwordGeneration.Get();

            Assert.True(password.Contains("N"));
            Assert.True(password.Any(x => !x.Equals('N')));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void GiveALengthLessThanTheNumberOfSuppliers_ArgumentExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => new PasswordGeneration(0, new List<ICharacters> { new LowercaseLetters() }));
        }
    }
}
