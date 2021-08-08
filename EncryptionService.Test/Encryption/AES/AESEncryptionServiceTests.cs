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
        
        [Fact]
        public void Can_Encrypt_And_Decrypt_Value()
        {
            var key = _aesKeyCreator.Create(1);
            _mockKeyManager.Setup(x => x.GetLatest()).Returns(key);
            _mockKeyManager.Setup(x => x.GetByVersion(key.Version)).Returns(key);

            var stringToEncrypt = "valueToEncrypt";
            var encryptedValue = _aesEncryptionService.Encrypt(stringToEncrypt);
            var decryptionResult = _aesEncryptionService.Decrypt(encryptedValue);

            decryptionResult.Should().BeOfType<SucceededDecryptionResult>();
            (decryptionResult as SucceededDecryptionResult)!.DecryptedValue.Should().Be(stringToEncrypt);
        }
    }
}