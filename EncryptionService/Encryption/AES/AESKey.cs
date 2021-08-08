using EncryptionService.Encryption.Keys;

namespace EncryptionService.Encryption.AES
{
    public class AESKey : IKey
    {
        private readonly byte[] _keyBytes;

        public AESKey(byte[] keyBytes, int version)
        {
            _keyBytes = keyBytes;
            Version = version;
        }

        public byte[] GetBytes() => _keyBytes;
        
        public int Version { get; }
    }
}