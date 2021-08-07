using EncryptionService.Controllers.Models;
using EncryptionService.Encryption;
using Microsoft.AspNetCore.Mvc;

namespace EncryptionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EncryptionController : ControllerBase
    {
        private readonly IEncryptionService _encryptionService;

        public EncryptionController(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        [HttpPost("encrypt")]
        public string Encrypt([FromBody] Value value)
        {
            return _encryptionService.Encrypt(value.StringValue);
        }
        
        [HttpPost("decrypt")]
        public string Decrypt([FromBody] Value value)
        {
            return _encryptionService.Decrypt(value.StringValue);
        }
    }
}