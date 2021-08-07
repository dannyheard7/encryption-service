using EncryptionService.Encryption;
using FluentAssertions;
using Xunit;

namespace EncryptionService.Test.Encryption
{
    public class SucceededDecryptionResultTests
    {
        [Fact]
        public void Succeeded_Should_Be_True()
        {
            var result = SucceededDecryptionResult.WithValue("somevalue");
            result.Succeeded.Should().BeTrue();
        }
    }
}