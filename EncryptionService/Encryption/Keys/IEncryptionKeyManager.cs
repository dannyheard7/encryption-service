namespace EncryptionService.Encryption.Keys
{
    public interface IEncryptionKeyManager
    {
        public IKey GetLatest();

        public void Rotate(IEncryptionKeyCreator<IKey> creator);
    }
}