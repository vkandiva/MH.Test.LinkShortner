using MH.Test.LinkShortner.WebAPIs.BusinessLogic;

namespace MH.Test.LinkShortner.WebAPIs.UnitTests
{
    using NUnit.Framework;

    namespace MH.Test.LinkShortner.Tests
    {
        [TestFixture]
        public class RandomStringGeneratorTests
        {
            [TestCase(0)]
            [TestCase(1)]
            [TestCase(10)]
            [TestCase(50)]
            public void GetRandomString_ValidLength(int numDigits)
            {
                // Act
                var result = RandomStringGenerator.GetRandomString(numDigits);

                // Assert
                Assert.AreEqual(numDigits, result.Length);
            }

            [TestCase(-1)]
            [TestCase(-5)]
            [TestCase(-15)]
            public void GetRandomString_NegativeLength(int numDigits)
            {
                // Act
                var result = RandomStringGenerator.GetRandomString(numDigits);

                // Assert
                Assert.AreEqual(string.Empty, result);
            }

            // Test to verify only valid characters are used in the generated string
            [TestCase(15)]
            public void GetRandomString_ShouldContainOnlyAvailableCharacters(int numDigits)
            {
                // Arrange
                const string availableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

                // Act
                var result = RandomStringGenerator.GetRandomString(numDigits);

                // Assert
                foreach (char c in result)
                {
                    Assert.Contains(c, availableCharacters.ToCharArray(), "Generated string contains invalid characters.");
                }
            }
        }
    }

}