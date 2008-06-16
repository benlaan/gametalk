using System;
using System.IO;
using System.Security.Cryptography;

namespace Laan.Utilities.Encryption
{
    public class Cypher
    {

        private static PasswordDeriveBytes GetPasswordDeriveBytes(string password)
        {
            byte[] salt = new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76};
            return new PasswordDeriveBytes(password, salt);
        }

        private static byte[] InternalEncrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                Rijndael alg = Rijndael.Create();

                alg.Key = Key;
                alg.IV = IV;

                using(CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // Write the data and make it do the encryption
                    cs.Write(clearData, 0, clearData.Length);
                    cs.Close();
                }

                return ms.ToArray();
            }
        }

        public static string Encrypt(string clearText, string password)
        {
            // First we need to turn the input string into a byte array.
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);

            PasswordDeriveBytes pdb = GetPasswordDeriveBytes(password);
            byte[] encryptedData = InternalEncrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);
        }

        public static byte[] Encrypt(byte[] clearData, string password)
        {
            PasswordDeriveBytes pdb = GetPasswordDeriveBytes(password);
            return InternalEncrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        // Decrypt a byte array into a byte array using a key and an IV
        private static byte[] InternalDecrypt(byte[] cipherData, byte[] Key, byte[] IV) 
        { 
            using(MemoryStream ms = new MemoryStream())
            {
                Rijndael alg = Rijndael.Create();

                alg.Key = Key;
                alg.IV = IV;
    
                using(CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherData, 0, cipherData.Length);
                    cs.Close();
                }
                return ms.ToArray();
            }
        }

        public static string Decrypt(string cipherText, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes pdb = GetPasswordDeriveBytes(password);

            byte[] decryptedData = InternalDecrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        public static byte[] Decrypt(byte[] cipherData, string password)
        {
            PasswordDeriveBytes pdb = GetPasswordDeriveBytes(password);
            return InternalDecrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }
    }

}
