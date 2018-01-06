using System;
using System.Security.Cryptography;

namespace Crypto
{
    public static class SecureRandom
    {
        private static readonly RandomNumberGenerator m_rng = new RNGCryptoServiceProvider();

        public static byte[] GetBytes(byte[] buffer, int offset, int count)
        {
            m_rng.GetBytes(buffer, offset, count);
            return buffer;
        }
        public static byte[] GetBytes(byte[] buffer) => GetBytes(buffer, 0, buffer.Length);
        public static byte[] GetBytes(int count) => GetBytes(new byte[count], 0, count);
        public static int GetInt32() => BitConverter.ToInt32(GetBytes(sizeof(int)), 0);
        public static long GetInt64() => BitConverter.ToInt64(GetBytes(sizeof(long)), 0);
        public static uint GetUInt32() => BitConverter.ToUInt32(GetBytes(sizeof(uint)), 0);
        public static ulong GetUInt64() => BitConverter.ToUInt64(GetBytes(sizeof(ulong)), 0);
    }
}