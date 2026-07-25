using Xunit;
using FluentAssertions;

namespace SchoolERP.Tests.UnitTests.Domain.Entities
{
    public class SimpleTest
    {
        [Fact]
        public void Test_ShouldPass()
        {
            // Arrange
            var expected = 5;

            // Act
            var actual = 2 + 3;

            // Assert
            actual.Should().Be(expected);
        }
    }
}