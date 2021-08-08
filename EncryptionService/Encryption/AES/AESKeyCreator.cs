using System;
using System.Linq;
using System.Security.Cryptography;
using EncryptionService.Encryption.Keys;

namespace EncryptionService.Encryption.AES
{
    public class AESKeyCreator : IEncryptionKeyCreator<AESKey>
    {
        private readonly int[] _allowedKeySizes = new int[] {16, 24, 32};
        private readonly int _keySize;
        
        public AESKeyCreator(int keySize)
        {
            if (!_allowedKeySizes.Contains(keySize))
                throw new InvalidOperationException($"Key size is invalid");
            
            _keySize = keySize;
        }

        public AESKey Create(int version)
        {
            byte[] bytes = new byte[_keySize];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(bytes);

            return new AESKey(bytes, version);
        }
    }
}