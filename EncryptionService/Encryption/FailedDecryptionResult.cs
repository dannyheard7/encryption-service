namespace EncryptionService.Encryption
{
    public class FailedDecryptionResult : DecryptionResult
    {
        public FailedDecryptionResult(DecryptionError error) : base(false)
        {
            Error = error;
        }
        
        public DecryptionError Error { get; }
    }
}