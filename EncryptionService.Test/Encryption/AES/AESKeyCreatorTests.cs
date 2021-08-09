using System;
using EncryptionService.Encryption.AES;
using FluentAssertions;
using Xunit;

namespace EncryptionService.Test.Encryption.AES
{
    public class AESKeyCreatorTests
    {
        [Theory]
        [InlineData(16)]
        [InlineData(24)]
        [InlineData(32)]
        public void Allows_Valid_KeySizes(int keySize)
        {
            var creator = new AESKeyCreator(keySize);
            creator.Create(1).Should().BeOfType<AESKey>();
        }
        
        [Fact]
        public void Throws_Exception_With_Invalid_KeySize()
        {
            Action act = () => new AESKeyCreator(1);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}