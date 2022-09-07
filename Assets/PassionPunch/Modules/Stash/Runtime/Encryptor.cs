using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PassionPunch.Stash
{
    public static class Encryptor
    {
        // Can be given anything
        private const string KEY_PHRASE = "os5yyDhHtQg4Oyaj1GGHPUtEWTO2NfBhtE3TE9LuwReZuSOGNUSq6wGeYE23fBnu0Bz4Te9CoPoB0t1MOCF4PcOWatRjIHHo8bFK";

        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        internal static string Encrypt(string plainText) // , string passPhrase)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainTextBytes));
        }

        internal static byte [] Encrypt(byte [] plainBytes)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();

            using(var password = new Rfc2898DeriveBytes(KEY_PHRASE, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);

                using(var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;

                    using(var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using(var memoryStream = new MemoryStream())
                        {
                            using(var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return cipherTextBytes;
                            }
                        }
                    }
                }
            }
        }

        internal static string Decrypt<T>(string encryptedText)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            byte [] plainTextBytes = Decrypt(encryptedBytes);

            return Encoding.UTF8.GetString(plainTextBytes); // , 0, plainTextBytes.Length);
        }

        internal static string Decrypt(string encryptedText)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            byte [] plainTextBytes = Decrypt(encryptedBytes);

            return Encoding.UTF8.GetString(plainTextBytes); // , 0, plainTextBytes.Length);
        }

        internal static byte [] Decrypt(byte [] encryptedBytes)
        {
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = encryptedBytes.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = encryptedBytes.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = encryptedBytes.Skip((Keysize / 8) * 2).Take(encryptedBytes.Length - ((Keysize / 8) * 2)).ToArray();

            using(var password = new Rfc2898DeriveBytes(KEY_PHRASE, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);

                using(var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;

                    using(var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using(var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using(var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte [cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return plainTextBytes;
                            }
                        }
                    }
                }
            }
        }

        private static byte [] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte [32]; // 32 Bytes will give us 256 bits.

            using(var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }

            return randomBytes;
        }
    }
}