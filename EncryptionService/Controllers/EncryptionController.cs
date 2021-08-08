using System;
using System.ComponentModel;
using System.Net;
using EncryptionService.Controllers.Models;
using EncryptionService.Encryption;
using Microsoft.AspNetCore.Mvc;

namespace EncryptionService.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class EncryptionController : ControllerBase
    {
        private readonly IEncryptionService _encryptionService;

        public EncryptionController(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        [HttpPost("encrypt")]
        [ProducesResponseType(typeof(Value),(int)HttpStatusCode.OK)]
        public ActionResult<Value> Encrypt([FromBody] Value value)
        {
            var encryptedValue = _encryptionService.Encrypt(value.StringValue);
            return new Value(encryptedValue);
        }
        
        [HttpPost("decrypt")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Value),(int)HttpStatusCode.OK)]
        public ActionResult<Value> Decrypt([FromBody] Value value)
        {
            var decryptedValue = _encryptionService.Decrypt(value.StringValue);

            if (decryptedValue is SucceededDecryptionResult succeededDecryptionResult)
                return new Value(succeededDecryptionResult.DecryptedValue);
            
            if (decryptedValue is FailedDecryptionResult failedDecryptionResult)
            {
                return failedDecryptionResult.Error switch
                {
                    DecryptionError.UnavailableEncryptionKey => new BadRequestObjectResult("Encryption key not available for this value"),
                    DecryptionError.IncorrectFormat => new BadRequestObjectResult("Value has incorrect format"),
                    _ => throw new InvalidEnumArgumentException(nameof(failedDecryptionResult.Error))
                };
            }

            throw new NotImplementedException($"No handler for result of type: {decryptedValue.GetType()}");
        }
        
        [HttpPost("rotateKey")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult RotateKey()
        {
            _encryptionService.RotateKey();
            return Ok();
        }
    }
}