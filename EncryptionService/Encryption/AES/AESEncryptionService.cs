using System;
using System.IO;
using System.Security.Cryptography;
using EncryptionService.Encryption.Keys;

namespace EncryptionService.Encryption.AES
{
    public class AESEncryptionService : IEncryptionService
    {
        private readonly IEncryptionKeyManager _encryptionKeyManager;

        public AESEncryptionService(IEncryptionKeyManager encryptionKeyManager)
        {
            _encryptionKeyManager = encryptionKeyManager;
        }

        private AESKey GetLatestKey()
        {
            var key = _encryptionKeyManager.GetLatest();
            if (key is not AESKey aesKey) throw new InvalidOperationException("Incorrect encryption key");

            return aesKey;
        }

        private byte[] CreateIv()
        {
            byte[] bytes = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(bytes);

            return bytes;
        }
        
        public string Encrypt(string value)
        {
            var aesKey = GetLatestKey();
            var iv = CreateIv();
            
            using var aes = Aes.Create();
            var encryptor = aes.CreateEncryptor(aesKey.GetBytes(), iv);

            using var memoryStream = new MemoryStream();
            using (CryptoStream csEncrypt = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(value);
                }
            }

            var encryptedData = memoryStream.ToArray();
            var encryptedResult = new AESEncryptionResult(encryptedData, iv, aesKey);
            return encryptedResult.ToString();
        }

        public DecryptionResult Decrypt(string value)
        {
            try
            {
                var decryptionRequest = new AESDecryptionRequest(value);
                var key = _encryptionKeyManager.GetByVersion(decryptionRequest.EncryptionKeyVersion);

                if (key is not AESKey aesKey) throw new InvalidOperationException("Incorrect encryption key");

                using var aes = Aes.Create();
                var transform = aes.CreateDecryptor(aesKey.GetBytes(), decryptionRequest.Iv);

                using MemoryStream memoryStream = new MemoryStream(decryptionRequest.EncryptedData);
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new StreamReader(cryptoStream);
                return SucceededDecryptionResult.WithValue(srDecrypt.ReadToEnd());
            }
            catch (FormatException)
            {
                return new FailedDecryptionResult(DecryptionError.IncorrectFormat);
            }
            catch (EncryptionKeyNotFoundException)
            {
                return new FailedDecryptionResult(DecryptionError.UnavailableEncryptionKey);
            }
        }
    }
}