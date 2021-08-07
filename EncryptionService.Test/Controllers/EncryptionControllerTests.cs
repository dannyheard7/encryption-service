using EncryptionService.Controllers;
using EncryptionService.Controllers.Models;
using EncryptionService.Encryption;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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

            result.Should().BeOfType<ActionResult<Value>>();
            result.Value.StringValue.Should().Be(encryptedValue);
        }
        
        [Fact]
        public void Decrypt_Returns_DecryptedValue_From_IEncryptionService()
        {
            var encryptedValue = new Value("encryptedValue");
            var decryptedValue = "decryptedValue";

            _mockEncryptionService
                .Setup(x => x.Decrypt(encryptedValue.StringValue))
                .Returns(SucceededDecryptionResult.WithValue(decryptedValue));
            
            var result = _encryptionController.Decrypt(encryptedValue);
            
            result.Should().BeOfType<ActionResult<Value>>();
            result.Value.StringValue.Should().Be(decryptedValue);
        }
        
        [Fact]
        public void Decrypt_Returns_BadRequest_If_EncryptedValue_EncryptionKey_Not_Available()
        {
            var encryptedValue = new Value("encryptedValue");

            _mockEncryptionService
                .Setup(x => x.Decrypt(encryptedValue.StringValue))
                .Returns(FailedDecryptionResult.UnavailableEncryptionKey);
            
            var result = _encryptionController.Decrypt(encryptedValue);
            
            result.Should().BeOfType<ActionResult<Value>>();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            (result.Result as BadRequestObjectResult)!.Value.Should().Be("Encryption key not available for this value");
        }
    }
}