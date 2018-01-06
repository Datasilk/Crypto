using System.Runtime.CompilerServices;

namespace Crypto
{
    public class Math
    {
        private const double INVERSE_32_BIT = 2.32830643653869628906e-010d;
        private const double INVERSE_52_BIT = 2.22044604925031308085e-016d;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double CombineIntoDouble(int x, int y)
        {
            return (0.5d + (INVERSE_52_BIT / 2) + (x * INVERSE_32_BIT) + ((y & 0x000FFFFF) * INVERSE_52_BIT));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long CombineIntoInt64(int lowPart, int highPart)
        {
            return ((long)CombineIntoUInt64(((uint)lowPart), ((uint)highPart)));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CombineIntoUInt64(uint lowPart, uint highPart)
        {
            return ((((ulong)highPart) << 32) | lowPart);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ClearBit(long value, int position)
        {
            return (value &= (~(1L << position)));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ClearBit(ulong value, int position)
        {
            return (value &= (~(1UL << position)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long CleaveHigh(long value)
        {
            return unchecked((int)value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CleaveHigh(ulong value)
        {
            return unchecked((uint)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long CleaveLow(long value)
        {
            return (value >>= 32);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CleaveLow(ulong value)
        {
            return (value >>= 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long FlipBit(long value, int position)
        {
            return (value ^= (1L << position));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong FlipBit(ulong value, int position)
        {
            return (value ^= (1UL << position));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBit(long value, int position)
        {
            return (MaskBit(value, position) != 0L);
        }

    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBit(ulong value, int position)
        {
            return (MaskBit(value, position) != 0UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(int value)
        {
            return ((value & 1) == 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(long value)
        {
            return ((value & 1L) == 1L);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(uint value)
        {
            return ((value & 1U) == 1U);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(ulong value)
        {
            return ((value & 1UL) == 1UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositive(int value)
        {
            return (value > 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositive(long value)
        {
            return (value > 0L);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositive(uint value)
        {
            return (value > 0U);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositive(ulong value)
        {
            return (value > 0UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOfTwo(long value)
        {
            return (IsPositive(value) && IsPowerOfTwo(unchecked((ulong)value)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOfTwo(ulong value)
        {
            return unchecked((value & ((~value) + 1UL)) == value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(long value)
        {
            return (value == 0L);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(ulong value)
        {
            return (value == 0UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long MaskBit(long value, int position)
        {
            return (value &= (1L << position));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong MaskBit(ulong value, int position)
        {
            return (value &= (1UL << position));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Mod16(long value)
        {
            return (value &= 15L);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Mod16(ulong value)
        {
            return (value &= 15UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Mod32(long value)
        {
            return (value &= 31L);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Mod32(ulong value)
        {
            return (value &= 31UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Mod64(long value)
        {
            return (value &= 63L);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Mod64(ulong value)
        {
            return (value &= 63UL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseBits(ref uint value)
        {
            var mask = 0x55555555U;
            value = (((value >> 1) & mask) | ((value & mask) << 1));
            mask = 0x33333333U;
            value = (((value >> 2) & mask) | ((value & mask) << 2));
            mask = 0X0F0F0F0FU;
            value = (((value >> 4) & mask) | ((value & mask) << 4));
            mask = 0X00FF00FFU;
            value = (((value >> 8) & mask) | ((value & mask) << 8));
            value = ((value >> 16) | (value << 16));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseBytes(ref uint value)
        {
            value = (
                ((value & 0x000000FFU) << 24)
              | ((value & 0x0000FF00U) << 8)
              | ((value & 0x00FF0000U) >> 8)
              | ((value & 0xFF000000U) >> 24)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, int count)
        {
            return ((value << count) | (value >> ((-count) & ((sizeof(uint) * 8) - 1))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(ulong value, int count)
        {
            return ((value << count) | (value >> ((-count) & ((sizeof(ulong) * 8) - 1))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(uint value, int count)
        {
            return ((value >> count) | (value << ((-count) & ((sizeof(uint) * 8) - 1))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateRight(ulong value, int count)
        {
            return ((value >> count) | (value << ((-count) & ((sizeof(ulong) * 8) - 1))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long SetBit(long value, int position)
        {
            return (value |= (1L << position));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong SetBit(ulong value, int position)
        {
            return (value |= (1UL << position));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Sign(long value)
        {
            return unchecked((value >> ((sizeof(long) * 8) - 1)) | ((((~value) + 1L) >> ((sizeof(long) * 8) - 1)) & 1L));
        }
    }
}
