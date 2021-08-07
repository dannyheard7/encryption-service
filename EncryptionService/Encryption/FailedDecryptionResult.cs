namespace EncryptionService.Encryption
{
    public class FailedDecryptionResult : DecryptionResult
    {
        private FailedDecryptionResult(DecryptionError error) : base(false)
        {
            Error = error;
        }
        
        public DecryptionError Error { get; }

        public static FailedDecryptionResult UnavailableEncryptionKey =>
            new FailedDecryptionResult(DecryptionError.UnavailableEncryptionKey);
    }
}