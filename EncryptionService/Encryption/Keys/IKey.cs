using System;

namespace EncryptionService.Encryption.Keys
{
    public interface IKey
    {
        public int Version { get; }
    }
}