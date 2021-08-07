using System;
using System.Text.RegularExpressions;

namespace EncryptionService.Encryption.AES
{
    public sealed class AESDecryptionRequest
    {
        public AESDecryptionRequest(string encryptedValueString)
        {
            // Match string that are in the format '[versionNumber]||[base64Iv]||[base64EncryptedString]'
            var encryptedStringFormatRegex = new Regex("^\\d+(\\|\\|)([^\\|\\|]+)(\\|\\|)([^\\|\\|]+)$");

            if (!encryptedStringFormatRegex.IsMatch(encryptedValueString))
                throw new FormatException("Value is not in correct format");

            var segments = encryptedValueString.Split("||");
            if (segments.Length != 3) throw new FormatException("Value is not in correct format");
            
            EncryptionKeyVersion = Int32.Parse(segments[0]);
            Iv = Convert.FromBase64String(segments[1]);
            EncryptedData = Convert.FromBase64String(segments[2]);
        }

        public byte[] EncryptedData { get; }
        public byte[] Iv { get; }
        public int EncryptionKeyVersion { get; }
    }
}