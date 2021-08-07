using System;
using EncryptionService.Encryption.Keys;

namespace EncryptionService.Encryption.AES
{
    public class AESKey : IKey
    {
        private readonly byte[] _keyBytes;

        public AESKey(byte[] keyBytes, int version, bool active, DateTime createdAt)
        {
            _keyBytes = keyBytes;
            Version = version;
            Active = active;
            CreatedAt = createdAt;
        }

        public byte[] GetBytes() => _keyBytes;
        
        public int Version { get; }
        public bool Active { get; }
        public DateTime CreatedAt { get; }
    }
}