namespace EncryptionService.Encryption
{
    public interface IEncryptionService
    {
        public string Encrypt(string value);

        public DecryptionResult Decrypt(string value);
    }
}