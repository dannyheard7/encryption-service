using EncryptionService.Encryption;
using FluentAssertions;
using Xunit;

namespace EncryptionService.Test.Encryption
{
    public class FailedDecryptionResultTests
    {
        [Fact]
        public void Succeeded_Should_Be_False()
        {
            var result = FailedDecryptionResult.UnavailableEncryptionKey;
            result.Succeeded.Should().BeFalse();
        }
    }
}