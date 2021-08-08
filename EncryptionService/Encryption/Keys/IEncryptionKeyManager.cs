namespace EncryptionService.Encryption.Keys
{
    public interface IEncryptionKeyManager<T> where T: class, IKey
    {
        public T GetLatest();
        
        public T GetByVersion(int version);

        public void Rotate(IEncryptionKeyCreator<T> creator);
    }
}