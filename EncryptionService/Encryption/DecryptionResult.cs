namespace EncryptionService.Encryption
{
    public abstract class DecryptionResult
    {
        protected DecryptionResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public bool Succeeded { get; }
    }
}