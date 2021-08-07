using System;
using EncryptionService.Encryption.AES;
using FluentAssertions;
using Xunit;

namespace EncryptionService.Test.Encryption.AES
{
    public class AESDecryptionRequestTests
    {
        [Fact]
        public void Should_Throw_A_FormatException_If_EncryptedString_Has_No_Separators()
        {
            Action act = () => new AESDecryptionRequest("randomformat");
            act.Should().Throw<FormatException>();
        }

        [Fact]
        public void Should_Throw_A_FormatException_If_EncryptedString_Has_Non_Numerical_Version()
        {
            Action act = () => new AESDecryptionRequest("version1||abc||def");
            act.Should().Throw<FormatException>();
        }
        
        [Fact]
        public void Should_Throw_A_FormatException_If_EncryptedString_Has_More_Than_Two_Separators()
        {
            Action act = () => new AESDecryptionRequest("1||abc||def||hij");
            act.Should().Throw<FormatException>();
        }
        
        [Fact]
        public void Should_Throw_A_FormatException_If_Iv_Is_Not_A_Base64String()
        {
            var base64String = Convert.ToBase64String(new byte[12]);
            Action act = () => new AESDecryptionRequest($"1||abc||{base64String}");
            act.Should().Throw<FormatException>();
        }
        
        [Fact]
        public void Should_Throw_A_FormatException_If_EncryptedData_Is_Not_A_Base64String()
        {
            var base64String = Convert.ToBase64String(new byte[12]);
            Action act = () => new AESDecryptionRequest($"1||{base64String}||abc");
            act.Should().Throw<FormatException>();
        }

        [Fact]
        public void Should_Populate_Fields_When_EncryptedString_Is_In_Correct_Format()
        {
            var ivByteData = new byte[12];
            var encryptionByteData = new byte[24];
            var version = 1;
            
            var decryptionRequest = new AESDecryptionRequest($"{version}||{Convert.ToBase64String(ivByteData)}||{Convert.ToBase64String(encryptionByteData)}");
            
            decryptionRequest.Iv.Should().Equal(ivByteData);
            decryptionRequest.EncryptedData.Should().Equal(encryptionByteData);
            decryptionRequest.EncryptionKeyVersion.Should().Be(version);
        }
    }
}