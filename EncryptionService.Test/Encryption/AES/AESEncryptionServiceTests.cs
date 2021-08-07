using System;
using EncryptionService.Encryption;
using EncryptionService.Encryption.AES;
using EncryptionService.Encryption.Keys;
using FluentAssertions;
using Moq;
using Xunit;

namespace EncryptionService.Test.Encryption.AES
{
    public class AESEncryptionServiceTests
    {
        private readonly Mock<IEncryptionKeyManager> _mockKeyManager;
        private readonly AESEncryptionService _aesEncryptionService;
        private readonly AESKeyCreator _aesKeyCreator;

        public AESEncryptionServiceTests()
        {
            _mockKeyManager = new Mock<IEncryptionKeyManager>();

            _aesKeyCreator = new AESKeyCreator();
            _aesEncryptionService = new AESEncryptionService(_mockKeyManager.Object);
        }

        [Fact]
        public void Encrypt_Uses_Latest_Key_From_IKeyManager()
        {
            var key = _aesKeyCreator.Create(1);
            _mockKeyManager.Setup(x => x.GetLatest()).Returns(key);
            
            _aesEncryptionService.Encrypt("value");
            
            _mockKeyManager.Verify(x => x.GetLatest(), Times.Once);
        }
        
        [Fact]
        public void Encrypt_Throws_Exception_If_Key_Is_Not_AESKey()
        {
            _mockKeyManager.Setup(x => x.GetLatest()).Returns((IKey)null);
            _aesEncryptionService.Invoking(x => x.Encrypt("value")).Should().Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void Can_Encrypt_And_Decrypt_Value()
        {
            var key = _aesKeyCreator.Create(1);
            _mockKeyManager.Setup(x => x.GetLatest()).Returns(key);

            var stringToEncrypt = "valueToEncrypt";
            var encryptedValue = _aesEncryptionService.Encrypt(stringToEncrypt);
            var decryptionResult = _aesEncryptionService.Decrypt(encryptedValue);

            decryptionResult.Should().BeOfType<SucceededDecryptionResult>();
            (decryptionResult as SucceededDecryptionResult)!.DecryptedValue.Should().Be(stringToEncrypt);
        }
    }
}