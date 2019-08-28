using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class Security
    {
        public static string EncryptBase64(string strSource)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strSource);
            string encryptedString = Convert.ToBase64String(b);
            return encryptedString;
        }

        public static string DecryptBase64(string strSource)
        {
            byte[] b = Convert.FromBase64String(strSource);
            string decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(b);
            return decryptedString;
        }

        public static string GeneratePassword(int Length)
        {
            if (Length > 32)
            {
                Length = 32;
            }
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, Length);
        }

        public static string GenerateSecretNo(int Length)
        {
            if (Length > 32)
            {
                Length = 32;
            }

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] ByteCode = new byte[Length];

            rng.GetBytes(ByteCode);

            StringBuilder SecurityCode = new StringBuilder();

            for (int i = 0; i < Length; i++)
            {
                SecurityCode.Append($"{(ByteCode[i] % 10)}");
            }

            return SecurityCode.ToString();
        }

        public static string ComputeHash(string plainText, HashAlgorithms Algorithm, byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
            {

                // Define min and max salt sizes.
                int minSaltSize = 0;
                int maxSaltSize = 0;

                minSaltSize = 4;
                maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = null;
                random = new Random();

                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            int I = 0;
            for (I = 0; I < plainTextBytes.Length; I++)
            {
                plainTextWithSaltBytes[I] = plainTextBytes[I];
            }

            // Append salt bytes to the resulting array.
            for (I = 0; I < saltBytes.Length; I++)
            {
                plainTextWithSaltBytes[plainTextBytes.Length + I] = saltBytes[I];
            }

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash = null;

            // Initialize appropriate hashing algorithm class.
            switch (Algorithm)
            {
                case HashAlgorithms.MD5:
                    hash = new MD5CryptoServiceProvider();
                    break;
                case HashAlgorithms.SHA1:
                    hash = new SHA1Managed();
                    break;
                case HashAlgorithms.SHA256:
                    hash = new SHA256Managed();
                    break;
                case HashAlgorithms.SHA384:
                    hash = new SHA384Managed();
                    break;
                case HashAlgorithms.SHA512:
                    hash = new SHA512Managed();
                    break;
                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (I = 0; I < hashBytes.Length; I++)
            {
                hashWithSaltBytes[I] = hashBytes[I];
            }

            // Append salt bytes to the result.
            for (I = 0; I < saltBytes.Length; I++)
            {
                hashWithSaltBytes[hashBytes.Length + I] = saltBytes[I];
            }

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        public static bool VerifyHash(string plainText, string hashValue, HashAlgorithms Algorithm)
        {
            //bool tempVerifyHash = false;

            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits = 0;
            int hashSizeInBytes = 0;

            // Size of hash is based on the specified algorithm.
            switch (Algorithm)
            {

                case HashAlgorithms.SHA1:
                    hashSizeInBits = 160;

                    break;
                case HashAlgorithms.SHA256:
                    hashSizeInBits = 256;

                    break;
                case HashAlgorithms.SHA384:
                    hashSizeInBits = 384;

                    break;
                case HashAlgorithms.SHA512:
                    hashSizeInBits = 512;

                    break;
                default: // Must be MD5
                    hashSizeInBits = 128;

                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = Convert.ToInt32(hashSizeInBits / 8.0);

            // Make sure that the specified hash value is long enough.
            //if (hashWithSaltBytes.Length < hashSizeInBytes)
            //{
            //    tempVerifyHash = false;
            //}

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            int I = 0;
            for (I = 0; I < saltBytes.Length; I++)
            {
                saltBytes[I] = hashWithSaltBytes[hashSizeInBytes + I];
            }

            // Compute a new hash string.
            string expectedHashString = ComputeHash(plainText, Algorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }
    }
    public enum HashAlgorithms
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }
}
