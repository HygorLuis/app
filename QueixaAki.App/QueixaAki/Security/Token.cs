using System;
using System.IO;
using System.Security.Cryptography;

namespace QueixaAki.Security
{
    public class Token
    {
        private readonly byte[] _key = { 45, 103, 73, 146, 210, 184, 220, 224, 94, 3, 114, 60, 211, 119, 21, 100, 18, 201, 230, 195, 119, 252, 73, 208, 209, 39, 222, 48, 47, 142, 94, 24 };
        private readonly byte[] _initializationVector = { 95, 17, 151, 243, 209, 243, 119, 80, 63, 252, 13, 180, 162, 13, 23, 218 };

        public bool VerifyToken(string authHeader)
        {
            try
            {
                if (!authHeader.Contains("Token")) return false;

                var authToken = authHeader.Replace("Token ", "");

                return DefaultDecryption(authToken);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string DefaultEncryption(long clientId, long providerId)
        {
            var keyToEncode = $"{providerId}:{DateTime.UtcNow.Ticks}";

            return Encrypt(keyToEncode);
        }

        public bool DefaultDecryption(string ecryptedText)
        {
            var textDecrypted = Decrypt(ecryptedText).Split(':');

            return DefaultVerification(textDecrypted);
        }

        public bool DefaultVerification(string[] textDecrypted)
        {
            if (textDecrypted.Length < 2) return false;

            var dateTimeTicks = long.Parse(textDecrypted[1]);

            var dateTime = new DateTime(dateTimeTicks, DateTimeKind.Utc).Date.AddHours(5);

            return dateTime >= DateTime.UtcNow.Date;
        }

        public string Encrypt(string plainText)
        {
            byte[] encrypted;

            using (var aesAlg = new AesCryptoServiceProvider())
            {
                var encryptor = aesAlg.CreateEncryptor(_key, _initializationVector);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string plainText)
        {
            var cipherText = Convert.FromBase64String(plainText);

            string textDecrypted;

            using (var aesAlg = new AesCryptoServiceProvider())
            {
                var decryptor = aesAlg.CreateDecryptor(_key, _initializationVector);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            textDecrypted = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return textDecrypted;
        }
    }
}
