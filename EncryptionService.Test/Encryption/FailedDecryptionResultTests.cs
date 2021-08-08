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
            var result = new FailedDecryptionResult(DecryptionError.IncorrectFormat);
            result.Succeeded.Should().BeFalse();
        }
    }
}