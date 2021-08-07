namespace EncryptionService.Encryption.Keys
{
    public interface IEncryptionKeyCreator<out T> where T : class, IKey
    {
        public T Create(int version);
    }
}