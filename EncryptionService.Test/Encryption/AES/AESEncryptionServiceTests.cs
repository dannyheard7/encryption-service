using System.Collections.Generic;
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
        private readonly Mock<IEncryptionKeyManager<AESKey>> _mockKeyManager;
        private readonly AESEncryptionService _aesEncryptionService;
        private readonly AESKeyCreator _aesKeyCreator;

        public AESEncryptionServiceTests()
        {
            _mockKeyManager = new Mock<IEncryptionKeyManager<AESKey>>();

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
        public void Decrypt_Returns_FailedDecryptionResult_UnavailableEncryptionKey_If_KeyVersion_DoesNotExist()
        {
            var key = _aesKeyCreator.Create(1);
            _mockKeyManager.Setup(x => x.GetByVersion(key.Version)).Throws<EncryptionKeyNotFoundException>();

            var valueToDecrypt = new AESEncryptionResult(new byte[2], new byte[12], key);
            var decryptionResult = _aesEncryptionService.Decrypt(valueToDecrypt.ToString());

            decryptionResult.Should().BeOfType<FailedDecryptionResult>();
            (decryptionResult as FailedDecryptionResult)!.Error.Should().Be(DecryptionError.UnavailableEncryptionKey);
        }
        
        [Fact]
        public void Decrypt_Returns_FailedDecryptionResult_IncorrectEncryptedValueFormat_If_DecryptedValue_Has_Wrong_Format()
        {
            var key = _aesKeyCreator.Create(1);
            _mockKeyManager.Setup(x => x.GetByVersion(key.Version)).Returns(key);
            
            var decryptionResult = _aesEncryptionService.Decrypt("somerandomformat");

            decryptionResult.Should().BeOfType<FailedDecryptionResult>();
            (decryptionResult as FailedDecryptionResult)!.Error.Should().Be(DecryptionError.IncorrectFormat);
        }
        
        public static IEnumerable<object[]> ValuesToEncrypt =>
            new List<string[]>
            {
                new string[] { "somevalue" },
                new string[] { "2321231" },
                new string[] { "" },
                new string[] { new string('a', 5000) }
            };
        
        [Theory]
        [MemberData(nameof(ValuesToEncrypt))]
        public void Can_Encrypt_And_Decrypt_Value(string valueToEncrypt)
        {
            var key = _aesKeyCreator.Create(1);
            _mockKeyManager.Setup(x => x.GetLatest()).Returns(key);
            _mockKeyManager.Setup(x => x.GetByVersion(key.Version)).Returns(key);
            
            var encryptedValue = _aesEncryptionService.Encrypt(valueToEncrypt);
            var decryptionResult = _aesEncryptionService.Decrypt(encryptedValue);

            decryptionResult.Should().BeOfType<SucceededDecryptionResult>();
            (decryptionResult as SucceededDecryptionResult)!.DecryptedValue.Should().Be(valueToEncrypt);
        }
        
        [Fact]
        public void Rotate_Calls_IKeyManager_Rotate_With_AESKeyCreator()
        {
            _aesEncryptionService.RotateKey();
            _mockKeyManager.Verify(x => x.Rotate(It.IsAny<AESKeyCreator>()), Times.Once);
        }
    }
}