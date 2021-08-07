using System;

namespace EncryptionService.Encryption.Keys
{
    public interface IKey
    {
        public int Version { get; }
        public bool Active { get; }
        public DateTime CreatedAt { get; }
    }
}