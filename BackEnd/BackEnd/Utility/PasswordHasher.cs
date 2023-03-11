using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BackEnd.Utility
{
    public static class PasswordHasher
    {
        public static byte[] HashMethod(string password, int numberOfIterations)
        {
            var rng = RandomNumberGenerator.Create();
            KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA256;
            int saltSize = 128 / 8;//128 bits

            byte[] salt = new byte[saltSize];
            rng.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, prf, numberOfIterations, 256 / 8);

            var outputBytes = new byte[8 + salt.Length + subkey.Length];

            ReadNetworkByteOrder(outputBytes, 0, (uint)numberOfIterations);
            ReadNetworkByteOrder(outputBytes, 4, (uint)saltSize);

            Buffer.BlockCopy(salt, 0, outputBytes, 8, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 8 + saltSize, subkey.Length);

            return outputBytes;
        }

        private static void ReadNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);
            try
            {
                // Read header information
                KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA256;
                int numberOfIterations = (int)ReadNetworkByteOrder(decodedHashedPassword, 0);
                int saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 4);

                // Read the salt: must be >= 128 bits
                if (saltLength < 128 / 8)
                {
                    return false;
                }
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(decodedHashedPassword, 8, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits
                int subkeyLength = decodedHashedPassword.Length - 8 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }
                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(decodedHashedPassword, 8 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                // Hash the incoming password and verify it
                byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, numberOfIterations, subkeyLength);
                return ByteArraysEqual(actualSubkey, expectedSubkey);
            }
            catch
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}
