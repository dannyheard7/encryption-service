namespace EncryptionService.Encryption.Keys
{
    public interface IEncryptionKeyManager
    {
        public IKey GetLatest();
        
        public IKey GetByVersion(int version);

        public void Rotate(IEncryptionKeyCreator<IKey> creator);
    }
}