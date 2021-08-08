using System;
using EncryptionService.Encryption.AES;
using FluentAssertions;
using Xunit;

namespace EncryptionService.Test.Encryption.AES
{
    public class AESEncryptionResultTests
    {
        private const int KeySize = 32;
        private readonly AESKeyCreator _aesKeyCreator;

        public AESEncryptionResultTests()
        {
            _aesKeyCreator = new AESKeyCreator(KeySize);
        }

        [Fact]
        public void ToString_Converts_Data_To_Formatted_EncryptionString()
        {
            var encryptionByteData = new byte[24];
            var ivByteData = new byte[12];
            var key = _aesKeyCreator.Create(1);

            var encryptionResult = new AESEncryptionResult(encryptionByteData, ivByteData, key);
            encryptionResult.ToString().Should().Be($"{key.Version}||{Convert.ToBase64String(ivByteData)}||{Convert.ToBase64String(encryptionByteData)}");
        }
    }
}