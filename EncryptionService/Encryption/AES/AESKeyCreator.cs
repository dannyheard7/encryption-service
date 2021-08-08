using System.Security.Cryptography;
using EncryptionService.Encryption.Keys;

namespace EncryptionService.Encryption.AES
{
    public class AESKeyCreator : IEncryptionKeyCreator<AESKey>
    {
        public AESKey Create(int version)
        {
            byte[] bytes = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(bytes);

            return new AESKey(bytes, version);
        }
    }
}