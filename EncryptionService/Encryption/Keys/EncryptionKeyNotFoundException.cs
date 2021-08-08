using System;

namespace EncryptionService.Encryption.Keys
{
    public class EncryptionKeyNotFoundException : Exception
    {
        public EncryptionKeyNotFoundException() : base("Encryption key could not be found")
        {
        }
    }
}