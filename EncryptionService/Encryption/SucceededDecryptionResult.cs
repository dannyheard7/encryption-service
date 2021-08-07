namespace EncryptionService.Encryption
{
    public class SucceededDecryptionResult : DecryptionResult
    {
        private SucceededDecryptionResult(string decryptedValue) : base(true)
        {
            DecryptedValue = decryptedValue;
        }
        
        public string DecryptedValue { get; }

        public static SucceededDecryptionResult WithValue(string value) => new SucceededDecryptionResult(value);
    }
}