using System;

namespace EncryptionService.Encryption.AES
{
    public sealed class AESEncryptionResult
    {
        public AESEncryptionResult(byte[] encryptedValue, byte[] iv, AESKey encryptionKey)
        {
            _encryptedValue = encryptedValue;
            _iv = iv;
            _encryptionKeyVersion = encryptionKey.Version;
        }

        private byte[] _encryptedValue { get; }
        private byte[] _iv { get; }
        private int _encryptionKeyVersion { get; }

        public override string ToString()
        {
            var valueAsBase64String = Convert.ToBase64String(_encryptedValue);
            var ivAsBase64String = Convert.ToBase64String(_iv);

            return $"{_encryptionKeyVersion}||{ivAsBase64String}||{valueAsBase64String}";
        }
    }
}