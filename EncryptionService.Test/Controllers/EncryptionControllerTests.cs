using EncryptionService.Controllers;
using EncryptionService.Controllers.Models;
using EncryptionService.Encryption;
using FluentAssertions;
using Moq;
using Xunit;

namespace EncryptionService.Test.Controllers
{
    public class EncryptionControllerTests
    {
        private readonly Mock<IEncryptionService> _mockEncryptionService;
        private readonly EncryptionController _encryptionController;

        public EncryptionControllerTests()
        {
            _mockEncryptionService = new Mock<IEncryptionService>();

            _encryptionController = new EncryptionController(_mockEncryptionService.Object);
        }

        [Fact]
        public void Encrypt_Returns_EncryptedValue_From_IEncryptionService()
        {
            var decryptedValue = new Value("valuetoencrypt");
            var encryptedValue = "encryptedValue";

            _mockEncryptionService.Setup(x => x.Encrypt(decryptedValue.StringValue)).Returns(encryptedValue);
            
            var result = _encryptionController.Encrypt(decryptedValue);
            
            result.Should().Be(encryptedValue);
        }
        
        [Fact]
        public void Decrypt_Returns_DecryptedValue_From_IEncryptionService()
        {
            var encryptedValue = new Value("encryptedValue");
            var decryptedValue = "decryptedValue";

            _mockEncryptionService.Setup(x => x.Decrypt(encryptedValue.StringValue)).Returns(decryptedValue);
            
            var result = _encryptionController.Decrypt(encryptedValue);
            
            result.Should().Be(decryptedValue);
        }
    }
}