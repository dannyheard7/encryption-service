namespace EncryptionService.Encryption.Keys
{
    public interface IKeyManager
    {
        public IKey GetLatest();

        public void Rotate();
    }
}