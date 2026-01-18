using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace AppReportesCcuFive9.Models
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string clearText, string encryptionKey)
        {
            string EncryptionKey = encryptionKey;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                var saltValueBytes = System.Text.Encoding.ASCII.GetBytes(EncryptionKey);
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, saltValueBytes);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText, string encryptionKey)
        {
            string EncryptionKey = encryptionKey;
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                var saltValueBytes = System.Text.Encoding.ASCII.GetBytes(EncryptionKey);
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, saltValueBytes);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}